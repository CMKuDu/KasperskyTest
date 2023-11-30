using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Queries.Purs.GetPurById
{
    public class GetPurByIdQueryHandler : IQueryHandler<GetPursByIdQuery, PurchaseHistory>
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        public GetPurByIdQueryHandler(IPurchaseHistoryService purchaseHistoryService)
        {   
            _purchaseHistoryService = purchaseHistoryService;
        }
        public async Task<PurchaseHistory> Handle(GetPursByIdQuery request, CancellationToken cancellationToken)
        {
            var pur = await _purchaseHistoryService.GetById(request.SubscriptionId);
            return pur;
        }

    }
}
