using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Infastruture.Repository
{
    public interface ILogRepository : IGenericRepository<ChangeLog>
    {
        void AddLog(ChangeLog log);
    }
    public class LogRepository : GenericRepository<ChangeLog>, ILogRepository
    {
        public LogRepository(ApiDbContext _context): base(_context) { }

        public void AddLog(ChangeLog log)
        {
            _context.Set<ChangeLog>().AddAsync(log);
        }
    }
}
