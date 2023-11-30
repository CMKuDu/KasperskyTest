using TestTelcoHub.Constant;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Model.ViewModel;

namespace TestTelcoHub.Service.Interface
{
    public interface IPurchaseHistoryService
    {
        Task<IEnumerable<PurchaseHistory>> GetAll();
        Task<PurchaseHistory> GetById(string subscriptionId);
        Task<ReponseBase> ModifyExpiration(ModifyExpirationViewModel model);
        Task<ReponseBase> ModifyQuantity(ModifyQuantityViewModel model);
        Task<bool> Update(PurchaseHistory purchaseHistory);
        Task<List<PurchaseHistory>> GetPurchaseHistory();
        //Task UpdatePurchaseHistories();
    }
}
