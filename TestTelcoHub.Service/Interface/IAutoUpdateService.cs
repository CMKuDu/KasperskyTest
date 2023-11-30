namespace TestTelcoHub.Service.Interface
{
    public interface IAutoUpdateService
    {
        Task AutoUpdateNodes();
        Task UpdatePurchaseHistories();
        //Task CheckAndUpdateSubscriptions();
    }
}
