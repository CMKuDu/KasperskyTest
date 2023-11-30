using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Queries.Purs.GetPursByUser
{
    public class GetPursByUserHandler : IQueryHandler<GetPursByUserQuery, IEnumerable<PurchaseHistory>>
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        public GetPursByUserHandler(IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
        }
        public async Task<IEnumerable<PurchaseHistory>> Handle(GetPursByUserQuery request, CancellationToken cancellationToken)
        {
            var pur = await _purchaseHistoryService.GetPurchaseHistory();
            return pur;
        }
    }
}
