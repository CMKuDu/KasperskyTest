using System.Windows.Input;
using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.Prototype;
using TestTelcoHub.Model.ViewModel;

namespace TestTelcoHub.Command.Product.Update.Renew
{
    public class RenewCommand : ICommand<ReponseBase>
    {
        public HardCancleRq Renew { get; set; }
        public RenewCommand(HardCancleRq renew) { Renew = renew; }
    }
}
