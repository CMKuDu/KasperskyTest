using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Service.Interface
{
    public interface ILogService
    {
        void LogQuantityChange(int oldQuantity, int newQuantity, string subscriptionId,string jsonData);
        void LogExpirationChange(DateTime? oldExpiration, DateTime? newExpiration, string subscriptionId, string jsonData);
        void LogHardCancle(string subscriptionId, string jsonData);
        void LogRenew(string subscriptionId, string jsonData);
        void LogError(string errorMessage, string jsonData);
        void LogEvent(string message, string action, string subscriptionId, string jsonData);
        Task<IEnumerable<ChangeLog>> GetAllLogs();
    }
}
