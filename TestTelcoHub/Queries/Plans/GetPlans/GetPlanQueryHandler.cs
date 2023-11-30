using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Queries.Plans.GetPlans
{
    public class GetPlanQueryHandler :IQueryHandler<GetPlanQuery,IEnumerable<Plan>>
    {
        private readonly IBillingPlanService _billingPlanService;
        public GetPlanQueryHandler(IBillingPlanService billingPlanService)
        {
            _billingPlanService = billingPlanService;
        }

        public async Task<IEnumerable<Plan>> Handle(GetPlanQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var plans = await _billingPlanService.GetAll();
                return plans.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Handle method: {ex.Message}");
                throw;
            }
        }

    }
}
