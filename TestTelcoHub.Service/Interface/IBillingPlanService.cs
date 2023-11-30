using TestTelcoHub.Constant;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Model.ViewModel;

namespace TestTelcoHub.Service.Interface
{
    public interface IBillingPlanService
    {
        Task<ReponseBase> CreateProduct(PlanDTO plan);
        Task<IEnumerable<Plan>> GetAll();
        Task<ReponseBase> Renew(HardCancleRq id);
        Task<ReponseBase> HardCancle(HardCancleRq hardCancle);
    }
}
