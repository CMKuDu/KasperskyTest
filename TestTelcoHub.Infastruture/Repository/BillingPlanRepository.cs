using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Infastruture.Repository
{
    public interface IBillingPlanRepository: IGenericRepository<Plan>
    {

    }   
    public class BillingPlanRepository : GenericRepository<Plan>, IBillingPlanRepository
    {
        public BillingPlanRepository(ApiDbContext _context) :base(_context){ }
    }
}
