using Newtonsoft.Json;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Service.Service
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public void LogHardCancle(string subscriptionId, string? jsonData)
        {
            _unitOfWork.Log.AddLog(new ChangeLog
            {
                Message = "Subscription hard canceled",
                CreateLog = DateTime.Now.ToString("yyyy/MM/dd"),
                Action = "HardCancle",
                SubscriptionId = subscriptionId,
                Data = jsonData,
            });

            _unitOfWork.Compele();
        }
        public void LogRenew(string subscriptionId, string jsonData)
        {
            _unitOfWork.Log.AddLog(new ChangeLog
            {
                Message = "Subscription renewed",
                CreateLog = DateTime.Now.ToString("yyyy/MM/dd"),
                Action = "Renew",
                SubscriptionId = subscriptionId,
                Data = jsonData,
            });

            _unitOfWork.Compele();
        }
        public void LogExpirationChange(DateTime? oldExpiration, DateTime? newExpiration, string subscriptionId, string jsonData)
        {
            _unitOfWork.Log.AddLog(new ChangeLog
            {
                Message = $"Thời hạn đăng ký thay đổi từ {oldExpiration?.ToString() ?? "null"} thành {newExpiration}",
                Action = "ChangeExpiration",
                CreateLog = DateTime.Now.ToString("yyyy/MM/dd"),
                SubscriptionId = subscriptionId,
                newValue = newExpiration.ToString()!,
                oldValue = (oldExpiration.HasValue ? oldExpiration.Value : DateTime.MinValue).ToString(),
                Data = jsonData, // Truyền dữ liệu JSON vào Data
            });
           _unitOfWork.Compele();
        }

        public void LogQuantityChange(int oldQuantity, int newQuantity, string subscriptionId, string jsonData)
        {
            var quantityChange = newQuantity - oldQuantity;
            _unitOfWork.Log.AddLog(new ChangeLog
            {
                Message = quantityChange > 0
                ? $"Số lượng nodes tăng từ {oldQuantity} lên {newQuantity}"
                : $"Số lượng nodes giảm từ {oldQuantity} xuống {newQuantity}",
                CreateLog = DateTime.Now.ToString("yyyy/MM/dd"),
                Action = "QuantityChange",
                newValue = newQuantity.ToString(),
                oldValue = oldQuantity.ToString(),
                SubscriptionId = subscriptionId,
                Data = jsonData,
            });
            _unitOfWork.Compele();
        }
        public void LogError(string errorMessage, string jsonData)
        {
            _unitOfWork.Log.AddLog(new ChangeLog
            {
                Message = $"Error: {errorMessage}",
                CreateLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Action = "Error",
                Data = jsonData,
            });

            _unitOfWork.Compele();
        }
        public void LogEvent(string message, string action, string subscriptionId, string jsonData)
        {
            string formattedJsonData = JsonConvert.SerializeObject(
                    JsonConvert.DeserializeObject(jsonData),
                    Formatting.Indented
                );
            _unitOfWork.Log.AddLog(new ChangeLog
            {
                Message = message,
                Action = action,
                CreateLog = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                SubscriptionId = subscriptionId,
                Data = formattedJsonData,
            });
            _unitOfWork.Compele();
        }
        public async Task<IEnumerable<ChangeLog>> GetAllLogs()
        {
            return await _unitOfWork.Log.GetAll();
        }
    }
}
