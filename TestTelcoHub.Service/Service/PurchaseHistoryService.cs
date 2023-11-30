using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Infastruture.Lib;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Service.Service
{
    public class PurchaseHistoryService : IPurchaseHistoryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly ILogService _logService;
        private readonly ApiDbContext _context;
        public PurchaseHistoryService(IUnitOfWork unitOfWork,
            IHttpClientHelper httpClientHelper,
            IHttpContextAccessor httpContextAccessor,
            ApiDbContext context, ILogService logService)
        {
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _httpClientHelper = httpClientHelper;
            _context = context;
        }
        public async Task<IEnumerable<PurchaseHistory>> GetAll()
        {
            try
            {
                return await _unitOfWork.Pur.GetAll();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi ở đây, có thể ghi log hoặc throw exception mới tùy vào yêu cầu của bạn.
                Console.WriteLine($"Error in GetAll method: {ex.Message}");
                throw;
            }
        }
        public async Task<PurchaseHistory> GetById(string subscriptionId)
        {
            var sub = await _unitOfWork.Pur.GetBySubscriptionId(subscriptionId);
            return sub;
        }
        public async Task<ReponseBase> ModifyExpiration(ModifyExpirationViewModel model)
        {
            var purchaseHistory = await _unitOfWork.Pur.GetBySubscriptionId(model.SubscriptionId);

            if (purchaseHistory == null)
            {
                return new ReponseBase { Status = ResponseContants.Error, Message = ProductConst.ProductExits };
            }

            // Cập nhật thông tin Expiration từ ModifyExpirationViewModel
            var oldExpiration = new Expiration
            {
                ExactMoment = purchaseHistory.ExactMoment,
                AfterMoment = purchaseHistory.AfterMoment,
                PeriodCount = purchaseHistory.PeriodCount
            };

            // Kiểm tra null trước khi chuyển đổi
            if (model.Expiration != null)
            {
                // Cập nhật thông tin Expiration từ ModifyExpirationViewModel
                purchaseHistory.ExactMoment = model.Expiration.ExactMoment;
                purchaseHistory.AfterMoment = model.Expiration.AfterMoment;
                purchaseHistory.PeriodCount = model.Expiration.PeriodCount;

                // Định dạng lại ngày theo ISO 8601
                var newExactMomentString = model.Expiration.ExactMoment?.ToString("yyyy-MM-dT00:00:00+00:00");
                var newAfterMomentString = model.Expiration.AfterMoment?.ToString("yyyy-MM-dT00:00:00+00:00");

                // Chuyển đổi ngày trước khi gán
                if (DateTime.TryParse(newExactMomentString, out DateTime newExactMoment))
                {
                    model.Expiration.ExactMoment = newExactMoment;
                }

                if (DateTime.TryParse(newAfterMomentString, out DateTime newAfterMoment))
                {
                    model.Expiration.AfterMoment = newAfterMoment;
                }

                // Serialize cả newValue và oldValue để lưu vào Log
                var jsonDatatwo = JsonSerializer.Serialize(new { newValue = model.Expiration, oldValue = oldExpiration });

                // Gọi phương thức LogExpirationChange để ghi Log
                _logService.LogExpirationChange(
                    purchaseHistory.AfterMoment ?? DateTime.MinValue,
                    purchaseHistory.ExactMoment,
                    model.SubscriptionId,
                    jsonDatatwo
                );

                using (var client = _httpClientHelper.CreateHttpClient())
                {
                    var endpoint = new Uri("https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/modifyexpiration");
                    var content = _httpClientHelper.CreateJsonContent(model);
                    var response = await _httpClientHelper.PostDataAsync(client, endpoint, content);
                    var jsonData = JsonSerializer.Serialize(model);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        // Lưu vào DB chỉ khi gửi yêu cầu thành công
                        bool dbUpdateSuccess = await UpdatePurchaseHistory(purchaseHistory);
                        if (dbUpdateSuccess)
                        {
                            // Trả về kết quả thành công khi cả hai thao tác đều thành công
                            return new ReponseBase { Status = ResponseContants.Success, Message = ModifyConst.ModifySuccess };
                        }
                        else
                        {
                            // Xử lý khi lưu vào DB không thành công
                            return new ReponseBase { Status = ResponseContants.Success, Message = "Gửi yêu cầu thành công nhưng lưu vào DB không thành công" };
                        }
                    }
                    else
                    {
                        var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(responseContent ?? "");
                        // Xử lý khi gửi yêu cầu không thành công
                        _logService.LogError($"Lỗi khi thay đổi thời gian: {errorResponse!.Message}", jsonData);
                        return new ReponseBase { Status = ResponseContants.Error, Message = ModifyConst.Modifyfail, Errors = new List<string> { errorResponse?.Message! } };
                    }
                }
            }
            else
            {
                // Xử lý khi model.Expiration là null
                return new ReponseBase { Status = ResponseContants.Error, Message = "Expiration information is missing" };
            }
        }
        public async Task<ReponseBase> ModifyQuantity(ModifyQuantityViewModel model)
        {
            var newQuantity = model.Quantity;
            var purchaseHistory = await _unitOfWork.Pur.GetBySubscriptionId(model.SubscriptionId!);

            if (purchaseHistory == null)
            {
                Log.Error("Không tìm thấy đơn hàng");
                return new ReponseBase
                {
                    Status = ResponseContants.Error,
                    Message = ProductConst.ProductExits
                };
            }
            var oldQuantity = purchaseHistory.Nodes;
            var jsonData = JsonSerializer.Serialize(model);
            var apiResult = await SendRequestToKaspersky(model);
            if (apiResult.Status == ResponseConst.Success)
            {
                _logService.LogQuantityChange(oldQuantity, newQuantity, model.SubscriptionId!, jsonData);
                if (newQuantity > oldQuantity)
                {
                    // Nếu số lượng nodes tăng, cập nhật và ghi log
                    purchaseHistory.Nodes = newQuantity;
                    _unitOfWork.Pur.Update(purchaseHistory);
                    _unitOfWork.Compele();
                    return new ReponseBase { Status = ResponseContants.Success, Message = ModifyConst.ModifySuccess };
                }
                else
                {
                    return new ReponseBase { Status = ResponseContants.Success, Message = ModifyConst.ModifySuccess };
                }
            }
            else
            {
                _logService.LogError($"Lỗi khi thay đổi số lượng nodes: {apiResult.Errors}", JsonSerializer.Serialize(model));
                return new ReponseBase { Status = ResponseContants.Error, Message = apiResult.Message, Errors = apiResult.Errors };
            }
        }
        public async Task<bool> Update(PurchaseHistory purchaseHistory)
        {
            var pur = await _unitOfWork.Pur.GetById(purchaseHistory.Id);
            if (pur != null)
            {
                pur.Nodes = purchaseHistory.Nodes;
                _unitOfWork.Pur.Update(pur);
                var result = _unitOfWork.Compele();

                return result > 0; // Nếu result > 0, cập nhật thành công, trả về true, ngược lại trả về false
            }
            return false;
        }
        public async Task<List<PurchaseHistory>> GetPurchaseHistory()
        {
            // Lấy userId từ thông tin đăng nhập
            var userPrincipal = _httpContextAccessor.HttpContext!.User;
            var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;

            if (userId == null)
            {
                // Xử lý khi không tìm thấy userId
                return new List<PurchaseHistory>();
            }
            try
            {
                var purchaseHistories = await _context.PurchaseHistories
                    .Where(x => x.AspNetUserId == userId)
                    .OrderByDescending(cb => cb.CreateDate)
                    .ToListAsync();
                return purchaseHistories;
            }
            catch (Exception ex)
            {
                // Xử lý khi có lỗi
                Console.WriteLine(ex.Message);
                return new List<PurchaseHistory>();
            }
        }
        private Task<bool> UpdatePurchaseHistory(PurchaseHistory purchaseHistory)
        {
            _unitOfWork.Pur.Update(purchaseHistory);
            var result = _unitOfWork.Compele();
            return Task.FromResult(result > 0);
        }
        private async Task<ReponseBase> SendRequestToKaspersky(ModifyQuantityViewModel model)
        {
            using (var client = _httpClientHelper.CreateHttpClient())
            {
                var endpoint = new Uri("https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/modifyquantity");
                var content = _httpClientHelper.CreateJsonContent(model);

                var response = await _httpClientHelper.PostDataAsync(client, endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    return new ReponseBase { Status = ResponseConst.Success, Message = ModifyConst.ModifySuccess };
                }
                else
                {
                    var subscriptionResponse = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(subscriptionResponse);
                    return new ReponseBase { Status = ResponseConst.Error, Message = ModifyConst.ModifyQuantityError, Errors = new List<string> { errorResponse?.Message! } };
                }
            }
        }
        //public async Task<KasperskyDetail> SendRequestToKasperskyDetails(string subscriptionId)
        //{
        //    using (var client = _httpClientHelper.CreateHttpClient())
        //    {
        //        var endpont = new Uri("https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/getdetails?SubscriptionId");
        //        var content = _httpClientHelper.CreateJsonContent(subscriptionId);
        //        var response = await _httpClientHelper.PostDataAsync(client, endpont, content);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null!;
        //        }
        //        else
        //        {
        //            var subscriptionResponse = await response.Content.ReadAsStringAsync();
        //            var subscriptionResponseDetail = JsonSerializer.Deserialize<KasperskyDetail>(subscriptionResponse);
        //            return subscriptionResponseDetail!;
        //        }
        //    }
        //}
        //public async Task UpdatePurchaseHistories()
        //{
        //    var purchaseHistories = await _context.PurchaseHistories
        //        .Where(ph => ph.ExactMoment != null && ph.ExactMoment.Value <= DateTime.UtcNow)
        //        .ToListAsync();

        //    foreach (var purchaseHistory in purchaseHistories)
        //    {
        //        // Gọi API Kaspersky để lấy thông tin chi tiết
        //        var kasperskyDetail = await SendRequestToKasperskyDetails(purchaseHistory.SubscriptionId);
        //        if (kasperskyDetail != null)
        //        {
        //            // Cập nhật thông tin trong bảng PurchaseHistory
        //            purchaseHistory.ExactMoment = kasperskyDetail.PeriodEnd; // không cần chuyển đổi
        //            purchaseHistory.PeriordType = (PeriordType)Enum.Parse(typeof(PeriordType), kasperskyDetail.PeriodType);
        //            purchaseHistory.StatusSubscription = (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), kasperskyDetail.Status);
        //            // Lưu thay đổi vào database
        //            _unitOfWork.Compele();
        //        }
        //    }
        //}
    }
}
