using TestTelcoHub.Infastruture.Interface;
using TestTelcoHub.Model.Data;

namespace TestTelcoHub.Infastruture.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        public IPurchaseHistoryRepository Pur { get; private set; }
        public IBillingPlanRepository Plan { get; private set; }
        public IRefreshTokenRepository RefreshToken { get; private set; }
        public ILogRepository Log { get; private set; }
        public IApprovalCodeRepository Approval { get; private set; }
        public UnitOfWork(ApiDbContext context,
            IApprovalCodeRepository approval,
            IBillingPlanRepository plan,
            IPurchaseHistoryRepository pur,
            IRefreshTokenRepository refreshToken,
            ILogRepository logRepository
            )
        {
            Approval = approval;
            Log = logRepository;
            RefreshToken = refreshToken;
            Pur = pur;
            Plan = plan;
            _context = context;
        }
        public int Compele()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                _context.Dispose();
            }
        }
    }
}
