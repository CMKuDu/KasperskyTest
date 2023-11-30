using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.DTOs;
using TestTelcoHub.Service.Constant;

namespace TestTelcoHub.Command.Product.Create
{
    public class CreateProductsCommand : ICommand<ReponseBase>
    {
        public PlanDTO PlanDTO { get; set; }

        public CreateProductsCommand(PlanDTO planDTO)
        {
            PlanDTO = planDTO;
        }
    }

}
