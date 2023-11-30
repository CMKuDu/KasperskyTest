using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Infastruture.Lib;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Service.Interface;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestTelcoHub.Service.Service
{
    public class BillingPlanService : IBillingPlanService
    {
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly ApiDbContext _context;
        public BillingPlanService(IUnitOfWork unitOfWork,
            IEmailService emailService,
            IMapper mapper,
            ApiDbContext context,
            IHttpClientHelper httpClientHelper,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogService logService)
        { 
            _logService = logService;
            _userManager = userManager;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
            _httpClientHelper = httpClientHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ReponseBase> CreateProduct(PlanDTO planDTO)
        {
            if (planDTO == null)
            {
                LogError("Invalid PlanDTO: null");
                return new ReponseBase { Status = ResponseContants.Error, Message = ProductConst.ProductCantNull };
            }
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ReponseBase { Status = ResponseContants.Error, Message = "User not found" };
            }
            var currentDate = DateTime.UtcNow;
            // Ánh xạ từ PlanDTO sang Plan
            var plan = _mapper.Map<Plan>(planDTO);
            await _unitOfWork.Plan.Add(plan);
            _unitOfWork.Compele();
            Guid planId = plan.PlanId;
            // Ánh xạ từ PlanDTO sang KasperskyVM
            var kasperskyVM = KasperskyMapper.MapFromPlanDTO(planDTO);
            using (var client = _httpClientHelper.CreateHttpClient())
            {
                var endpoint = new Uri("https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/create");
                var content = _httpClientHelper.CreateJsonContent(kasperskyVM);
                var response = await _httpClientHelper.PostDataAsync(client, endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var subscriptionResponse = JsonSerializer.Deserialize<ResposeResult>(responseContent);
                var agreementText = planDTO.TermsAndConditions!.CustomerAgreements?.FirstOrDefault()?.AgreementText;
                LogInformation($"Nội dung hợp đồng: {agreementText}");
                if (!response.IsSuccessStatusCode)
                {

                    var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(responseContent);
                    _logService.LogError(errorResponse!.Message, JsonSerializer.Serialize(planDTO));
                    return new ReponseBase
                    {
                        Status = ResponseContants.Error,
                        Message = CreateConst.CreateFail,
                        Errors = new List<string> { errorResponse?.Message ?? "Unknown error" },
                    };
                }
                _logService.LogEvent(ResponseContants.Success,"Create", subscriptionResponse!.SubscriptionId , JsonSerializer.Serialize(planDTO));
                // Chỉ định ngày hết hạn
                var exac = planDTO.ExpirationDTO?.ExactMoment ?? currentDate.AddMonths(1);
                var after = exac.AddDays(1);

                if (planDTO.ExpirationDTO == null)
                {
                    planDTO.ExpirationDTO = new()
                    {
                        MomentTypeDTO = 0,
                        ExactMoment = exac,
                        AfterMoment = after,
                        PeriodCount = 0
                    };
                }
                var purchaseHistory = CreatePurchaseHistory(userId, currentDate, subscriptionResponse!, planDTO, exac);
                await _unitOfWork.Pur.Add(purchaseHistory);
                _unitOfWork.Compele();
                await _emailService.SendSuccessEmailAsync(subscriptionResponse!, planDTO.DeliveryEmail);
                subscriptionResponse!.Success = true;
                return new ReponseBase
                {
                    Status = ResponseContants.Success,
                    Message = CreateConst.CreatedSuccessfully,
                    ExpirationInfo = new ExpirationInfo
                    {
                        ExactMoment = exac,
                        AfterMoment = after,
                    }
                };
            }
        }
        public async Task<IEnumerable<Plan>> GetAll()
        {
            return await _unitOfWork.Plan.GetAll();
        }
        public async Task<ReponseBase> Renew(HardCancleRq id)
        {
            if (id == null)
                return null!;
            using (var client = _httpClientHelper.CreateHttpClient())
            {
                Uri endpoint = new Uri("https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/renew");
                var content = _httpClientHelper.CreateJsonContent(id);

                // Lấy thông tin đăng nhập
                ClaimsPrincipal userPrincipal = _httpContextAccessor.HttpContext!.User;
                var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim?.Value!;

                string json = JsonSerializer.Serialize(id);
                HttpResponseMessage response = await _httpClientHelper.PostDataAsync(client, endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    if (userId != null)
                    {
                        var purchaseHistory = await _context.PurchaseHistories
                            .FirstOrDefaultAsync(x => x.AspNetUserId == userId && x.SubscriptionId == id.SubscriptionId);

                        if (purchaseHistory != null)
                        {
                            purchaseHistory.StatusSubscription = SubscriptionStatus.Active;
                            await _context.SaveChangesAsync();
                        }
                    }
                    //Ghi Log khi thanhf coong
                    _logService.LogEvent("Subscription renewed", "Renew", id.SubscriptionId!, JsonConvert.SerializeObject(id));
                    var subscriptionResponse = new ReponseBase
                    {
                        Status = ResponseContants.Success,
                        Message = RenewConst.RenewSuccessfully,
                    };
                    return subscriptionResponse;
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(responseContent);
                    //Gi Log neu loi
                    _logService.LogEvent($"Renew failed: {errorResponse?.Message}", "RenewFail", id.SubscriptionId!, JsonConvert.SerializeObject(id));
                    var subscriptionResponse = new ReponseBase
                    {
                        Status = ResponseContants.Error,
                        Message = RenewConst.RenewFail,
                        Errors = new List<string> { errorResponse?.Message! }
                    };
                    return subscriptionResponse;
                }
            }
        }
        public async Task<ReponseBase> HardCancle(HardCancleRq hardCancle)
        {
            if (hardCancle == null)
                return null!;

            using (var client = _httpClientHelper.CreateHttpClient())
            {
                var endpoint = new Uri("https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/hardcancel");
                var content = _httpClientHelper.CreateJsonContent(hardCancle);
                var response = await _httpClientHelper.PostDataAsync(client, endpoint, content);
                // Lấy thông tin đăng nhập
                var userPrincipal = _httpContextAccessor.HttpContext!.User;
                var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                var userId = userIdClaim?.Value;

                var jsonData = JsonSerializer.Serialize(hardCancle);
                if (response.IsSuccessStatusCode)
                {
                    _logService.LogEvent("Subscription hard canceled", "HardCancel", hardCancle.SubscriptionId!, jsonData);


                    if (userId != null)
                    {
                        // Lấy purchase history cần cập nhật
                        var purchaseHistory = await _context.PurchaseHistories
                            .FirstOrDefaultAsync(x => x.AspNetUserId == userId && x.SubscriptionId == hardCancle.SubscriptionId);

                        if (purchaseHistory != null)
                        {
                            // Cập nhật trạng thái thành HardCancel
                            purchaseHistory.StatusSubscription = SubscriptionStatus.HardCancel;
                            // Lưu thay đổi vào cơ sở dữ liệu
                            await _context.SaveChangesAsync();
                        }
                    }

                    var subscriptionResponse = new ReponseBase
                    {
                        Status = ResponseContants.Success,
                        Message = CancleConst.CacnleSuccessfully,
                    };
                    return subscriptionResponse;
                }
                else
                {
                    _logService.LogError(ResponseContants.Error, JsonSerializer.Serialize(hardCancle));
                    string errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponseModel>(errorContent);
                    _logService.LogError($"Subscription hard cancel failed: {errorResponse?.Message}", jsonData);
                    var subscriptionResponse = new ReponseBase
                    {
                        Status = ResponseContants.Error,
                        Message = CancleConst.CancleFail,
                        Errors = new List<string> { errorResponse?.Message! }
                    };
                    return subscriptionResponse;
                }
            }
        }
        private void LogInformation(string message)
        {
            // Địa điểm để ghi log thông tin (ví dụ: log file, database, ...)
            Console.WriteLine($"[Information] {message}");
        }
        private void LogError(string message)
        {
            // Địa điểm để ghi log lỗi (ví dụ: log file, database, ...)
            Console.WriteLine($"[Error] {message}");
        }
        public static class KasperskyMapper
        {
            public static KasperskyVM MapFromPlanDTO(PlanDTO planDTO)
            {
                if (planDTO == null)
                {
                    return null!;
                }
                var kasperskyVM = new KasperskyVM
                {
                    BillingPlan = planDTO.BillingPlan,
                    Sku = planDTO.Sku,
                    Quantity = planDTO.Quantity,
                    Expiration = null,
                    Customer = MapCustomer(planDTO.Customer!),
                    Distributor = new DistributorDTO
                    {
                        Partner = "NTS_ICT_Co",
                        Reseller = ""
                    },
                    ApprovalCode = planDTO.ApprovalCode,
                    DeliveryEmail = planDTO.DeliveryEmail,
                    TermsAndConditions = MapTermsAndConditions(planDTO!.TermsAndConditions!),
                    Comment = planDTO.Comment,
                    // Ánh xạ thêm các property khác nếu cần
                };
                return kasperskyVM;
            }
            private static CustomerDTO MapCustomer(CustomerDTO customerDTO)
            {
                if (customerDTO == null)
                {
                    return null!;
                }

                return new CustomerDTO
                {
                    Contacts = new ContactsDTO { CompanyName = customerDTO.Contacts!.CompanyName },
                    Address = new AddressDTO { Country = customerDTO.Address!.Country }
                };
            }
            private static TermsAndConditionsDTO MapTermsAndConditions(TermsAndConditionsDTO termsAndConditionsDTO)
            {
                if (termsAndConditionsDTO == null)
                {
                    return null!;
                }

                return new TermsAndConditionsDTO
                {
                    CustomerAgreements = termsAndConditionsDTO.CustomerAgreements
                        ?.Select(agreement => new CustomerAgreementsDTO
                        {
                            AgreementAccepted = agreement.AgreementAccepted,
                            AgreementText = agreement.AgreementText,
                            AgreementTextHash = agreement.AgreementTextHash
                        })
                        .ToList()
                };
            }
        }
        private PurchaseHistory CreatePurchaseHistory(string userId, DateTime currentDate, ResposeResult subscriptionResponse, PlanDTO planDTO, DateTime exac)
        {
            var purchaseHistory = _mapper.Map<PurchaseHistory>(planDTO);
            purchaseHistory.StatusSubscription = SubscriptionStatus.Active;
            purchaseHistory.CreateDate = currentDate.ToString("yyyy-MM-dd");
            purchaseHistory.SubscriptionId = subscriptionResponse!.SubscriptionId!;
            purchaseHistory.ActivationCode = subscriptionResponse!.ActivationCode!;
            purchaseHistory.LicenseId = subscriptionResponse!.LicenseId!;
            purchaseHistory.AspNetUserId = userId;
            purchaseHistory.PeriordType = PeriordType.Free;
            return purchaseHistory;
        }
        private string GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value!;
        }
        private ReponseBase CheckAccess()
        {
            var userPrincipal = _httpContextAccessor.HttpContext!.User;

            if (!userPrincipal.Identity!.IsAuthenticated)
            {
                LogError("Unauthorized access.");
                return new ReponseBase { Status = ResponseConst.Error, Message = "Unauthorized access" };
            }

            return new ReponseBase { Status = ResponseConst.Success };
        }
    }
}
