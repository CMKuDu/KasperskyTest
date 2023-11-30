using TestTelcoHub.Infastruture.Repository;

namespace TestTelcoHub.Infastruture.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IPurchaseHistoryRepository Pur { get; }
        IBillingPlanRepository Plan { get; }
        IRefreshTokenRepository RefreshToken { get; }
        ILogRepository Log { get; }
        IApprovalCodeRepository Approval { get; }
        int Compele();
    }
}
