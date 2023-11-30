using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.ViewModel;

namespace TestTelcoHub.Command.Product.Update.HardCancle
{
    public class HardCancleCommand : ICommand<ReponseBase>
    {
        public HardCancleRq Hardrq { get; set; }
        public HardCancleCommand(HardCancleRq hardCancleRq) { Hardrq = hardCancleRq; }
    }
}
