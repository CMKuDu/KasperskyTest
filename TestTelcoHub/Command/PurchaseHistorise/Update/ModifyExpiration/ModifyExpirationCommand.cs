using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.ViewModel;

namespace TestTelcoHub.Command.PurchaseHistorise.Update.ModifyExpiration
{
    public class ModifyExpirationCommand : ICommand<ReponseBase>
    {
        public ModifyExpirationViewModel Model { get; set; }
        public ModifyExpirationCommand(ModifyExpirationViewModel model) { Model = model; }
    }
}
