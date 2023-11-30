using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Infastruture.Repository
{
    public interface IApprovalCodeRepository : IGenericRepository<ApprovalCode>
    {
    }
    public class ApprovalCodeRepository : GenericRepository<ApprovalCode>, IApprovalCodeRepository
    {
        public ApprovalCodeRepository(ApiDbContext _context) : base(_context) { }
    }
}
