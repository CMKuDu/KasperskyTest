using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Service.Constant;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Command.Product.Create
{
    public class CreateProductsCommandHandler : ICommandHandler<CreateProductsCommand, ReponseBase>
    {
        private readonly IBillingPlanService _billingPlanService;
        public CreateProductsCommandHandler(IBillingPlanService billingPlanService)
        {
            _billingPlanService = billingPlanService;
        }

        public async Task<ReponseBase> Handle(CreateProductsCommand request, CancellationToken cancellationToken)
        {
            return await _billingPlanService.CreateProduct(request.PlanDTO);
        }
    }
}
