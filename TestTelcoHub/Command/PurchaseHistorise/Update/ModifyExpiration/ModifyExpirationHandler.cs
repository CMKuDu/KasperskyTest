using TestTelcoHub.Constant;
using TestTelcoHub.Infastruture.ICommand;
using TestTelcoHub.Model.ViewModel;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Command.PurchaseHistorise.Update.ModifyExpiration
{
    public class ModifyExpirationHandler : ICommandHandler<ModifyExpirationCommand, ReponseBase>
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        public ModifyExpirationHandler(IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
        }

        public async Task<ReponseBase> Handle(ModifyExpirationCommand request, CancellationToken cancellationToken)
        {
            return await _purchaseHistoryService.ModifyExpiration(request.Model);
        }
    }
}
