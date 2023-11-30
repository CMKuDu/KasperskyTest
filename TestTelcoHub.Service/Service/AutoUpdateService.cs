
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestTelcoHub.Infastruture.Lib;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Service.Service
{
    public class AutoUpdateService : BackgroundService, IAutoUpdateService
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<AutoUpdateService> _logger;
        private DateTime lastUpdateDateTime;

        public AutoUpdateService(
            IHttpClientHelper httpClientHelper,
            ILogger<AutoUpdateService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _httpClientHelper = httpClientHelper;
            lastUpdateDateTime = DateTime.UtcNow;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        public async Task UpdatePurchaseHistories()
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                // Lấy dịch vụ cần thiết từ service provider trong scope mới
                var serviceProvider = scope.ServiceProvider;
                var purchaseHistoryServices = serviceProvider.GetRequiredService<IPurchaseHistoryService>();
                var dbContext = serviceProvider.GetRequiredService<ApiDbContext>();
                var purchaseHistories = await dbContext.PurchaseHistories
                    .ToListAsync();
                foreach (var purchaseHistory in purchaseHistories)
                {
                    // Gọi API Kaspersky để lấy thông tin chi tiết
                    var kasperskyDetail = await SendRequestToKasperskyDetails(purchaseHistory.SubscriptionId);
                    if (kasperskyDetail != null)
                    {
                        // Cập nhật thông tin trong bảng PurchaseHistory
                        purchaseHistory.ExactMoment = kasperskyDetail.PeriodEnd; // không cần chuyển đổi
                        purchaseHistory.PeriordType = (PeriordType)Enum.Parse(typeof(PeriordType), kasperskyDetail.PeriodType);
                        purchaseHistory.StatusSubscription = (SubscriptionStatus)Enum.Parse(typeof(SubscriptionStatus), kasperskyDetail.Status);
                        // Lưu thay đổi vào database
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task AutoUpdateNodes()
        {
            try
            {
                _logger.LogInformation("Auto updating nodes...");
                // Tạo một scope mới
                using (var scope = _scopeFactory.CreateScope())
                {
                    // Lấy dịch vụ cần thiết từ service provider trong scope mới
                    var serviceProvider = scope.ServiceProvider;
                    var purchaseHistoryServices = serviceProvider.GetRequiredService<IPurchaseHistoryService>();
                    var dbContext = serviceProvider.GetRequiredService<ApiDbContext>();
                    // Lấy tất cả đơn hàng cần kiểm tra
                    var purchaseHistories = await dbContext.PurchaseHistories
                        .Where(ph => ph.ExactMoment != null && ph.ExactMoment.Value <= DateTime.UtcNow)
                        .ToListAsync();
                    foreach (var item in purchaseHistories)
                    {
                        if (item.StatusSubscription == SubscriptionStatus.Active)
                        {
                            var changeLog = await dbContext.ChangeLogs
                            .Where(sb => sb.SubscriptionId == item.SubscriptionId)
                            .OrderByDescending(sb => sb.CreateLog)
                            .FirstOrDefaultAsync();
                            if (changeLog != null)
                            {
                                var newQuantity = JsonConvert.DeserializeObject<int>(changeLog.newValue);
                                if (newQuantity != 0)
                                {
                                    item.Nodes = newQuantity;
                                    if (item.ExactMoment <= DateTime.UtcNow)
                                    {
                                        item.ExactMoment = DateTime.UtcNow.AddDays(30);
                                    }
                                    await purchaseHistoryServices.Update(item);
                                    _logger.LogInformation($"Auto updating nodes for purchase history ID: {item.Id}");
                                }
                            }
                        }
                    }
                }
                _logger.LogInformation("Auto update complete.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during auto update nodes: {ex.Message}");
            }
        }
        public async Task<KasperskyDetail> SendRequestToKasperskyDetails(string subscriptionId)
        {
            using (var client = _httpClientHelper.CreateHttpClient())
            {
                var endpont = new Uri($"https://api.demo.korm.kaspersky.com/Subscriptions/v2.0/api/Subscription/getdetails?SubscriptionId={Uri.EscapeDataString(subscriptionId)}");
                var content = _httpClientHelper.CreateJsonContent(subscriptionId);
                var response = await _httpClientHelper.PostDataAsync(client, endpont, content);
                if (!response.IsSuccessStatusCode)
                {
                    return null!;
                }
                else
                {
                    var subscriptionResponse = await response.Content.ReadAsStringAsync();
                    var subscriptionResponseDetail = JsonConvert.DeserializeObject<KasperskyDetail>(subscriptionResponse);
                    return subscriptionResponseDetail!;
                }
            }
        }
        //public async Task CheckAndUpdateSubscriptions()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Checking and updating subscriptions...");

        //        using (var scope = _scopeFactory.CreateScope())
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        //            var purchaseHistoryServices = scope.ServiceProvider.GetRequiredService<IPurchaseHistoryService>();

        //            var subscriptions = await dbContext.PurchaseHistories
        //                .Where(ph => ph.StatusSubscription == SubscriptionStatus.Active)
        //                .ToListAsync();
        //            foreach (var subscription in subscriptions)
        //            {
        //                if (subscription.PeriordType == PeriordType.Free)
        //                {
        //                    // Kiểm tra ExactMoment với thời điểm hiện tại đã là 1 tháng chưa
        //                    if (subscription.ExactMoment <= DateTime.UtcNow)
        //                    {
        //                        // Chuyển từ Free sang Paid và cập nhật ExactMoment
        //                        subscription.PeriordType = PeriordType.Paid;
        //                        subscription.ExactMoment = DateTime.UtcNow.AddYears(1); // hoặc DateTime.UtcNow.AddYears(1).AddDays(-1) nếu là năm nhuận
        //                        await purchaseHistoryServices.Update(subscription);
        //                        _logger.LogInformation($"Updated subscription ID: {subscription.SubscriptionId} from Free to Paid");
        //                    }
        //                }
        //                else if (subscription.PeriordType == PeriordType.Paid)
        //                {
        //                    // Kiểm tra ExactMoment với thời điểm hiện tại đã là 1 năm chưa
        //                    if (subscription.ExactMoment <= DateTime.UtcNow)
        //                    {
        //                        // Cập nhật lại ExactMoment
        //                        subscription.ExactMoment = DateTime.UtcNow.AddYears(1); // hoặc DateTime.UtcNow.AddYears(1).AddDays(-1) nếu là năm nhuận
        //                        await purchaseHistoryServices.Update(subscription);
        //                        _logger.LogInformation($"Updated subscription ID: {subscription.SubscriptionId} ExactMoment for Paid subscription");
        //                    }
        //                }
        //            }
        //        }
        //        _logger.LogInformation("Subscription check and update complete.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error during subscription check and update: {ex.Message}");
        //    }
        //}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                // Gọi AutoUpdateNodes sau mỗi 24 giờ
                //await CheckAndUpdateSubscriptions();
                await UpdatePurchaseHistories();
                await AutoUpdateNodes();
                // Đợi một khoảng thời gian trước khi thực hiện lại, ví dụ 24 giờ
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }

}
