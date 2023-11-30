using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Command.Product.Update.HardCancle
{
    public class HardCancleCommandHandler : ICommandHandler<HardCancleCommand, ReponseBase>
    {
        private readonly IBillingPlanService _billingPlanService;
        public HardCancleCommandHandler(IBillingPlanService billingPlanService)
        {
            _billingPlanService = billingPlanService;
        }
        public async Task<ReponseBase> Handle(HardCancleCommand request, CancellationToken cancellationToken)
        {
            return await _billingPlanService.HardCancle(request.Hardrq);
        }
    }
}
