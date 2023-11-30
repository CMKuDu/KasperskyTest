using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Command.PurchaseHistorise.Update.ModifyQuantity
{
    public class ModifyQuantityCommandHandler : ICommandHandler<ModifyQuantityCommand, ReponseBase>
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        public ModifyQuantityCommandHandler(IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
        }

        public async Task<ReponseBase> Handle(ModifyQuantityCommand request, CancellationToken cancellationToken)
        {
            return await _purchaseHistoryService.ModifyQuantity(request.ViewModel);
        }
    }
}
