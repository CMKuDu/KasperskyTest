using Microsoft.AspNetCore.Http;
using System.Data.Entity;
using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;


namespace TestTelcoHub.Infastruture.Repository
{
    public interface IPurchaseHistoryRepository : IGenericRepository<PurchaseHistory>
    {
        Task<PurchaseHistory> GetBySubscriptionId(string subscriptionId);
        Task<List<PurchaseHistory>> GetSubscriptionsByEmailAsync(string email);
    }
    public class PurchaseHistoryRepository : GenericRepository<PurchaseHistory>, IPurchaseHistoryRepository
    {
        public PurchaseHistoryRepository(ApiDbContext _context) : base(_context) { }


        public Task<PurchaseHistory> GetBySubscriptionId(string subscriptionId)
        {
            return GetFirstOrDefaultAsync(p => p.SubscriptionId == subscriptionId);
        }

        public async Task<List<PurchaseHistory>> GetSubscriptionsByEmailAsync(string email)
        {
            return await _context.PurchaseHistories
            .Where(p => p.DeliveryEmail == email)
            .ToListAsync();
        }
    }
}