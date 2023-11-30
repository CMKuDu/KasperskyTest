using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.ViewModel;

namespace TestTelcoHub.Command.PurchaseHistorise.Update.ModifyQuantity
{
    public class ModifyQuantityCommand : ICommand<ReponseBase>
    {
        public ModifyQuantityViewModel ViewModel { get; set; }
        public ModifyQuantityCommand(ModifyQuantityViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
