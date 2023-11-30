using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Command.Product.Update.Renew
{
    public class RenewCommandHandler : ICommandHandler<RenewCommand, ReponseBase>
    {
        private readonly IBillingPlanService _billingPlanService;
        public RenewCommandHandler(IBillingPlanService billingPlanService)
        {
            _billingPlanService = billingPlanService;
        }
        public async Task<ReponseBase> Handle(RenewCommand request, CancellationToken cancellationToken)
        {
            return await _billingPlanService.Renew(request.Renew);
        }
    }
}
