using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;
using TestTelcoHub.Service.Interface;

namespace TestTelcoHub.Queries.Purs.GetPurs
{
    public class GetPurPurHandler : IQueryHandler<GetPurQuery, IEnumerable<PurchaseHistory>>
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;
        public GetPurPurHandler(IPurchaseHistoryService purchaseHistoryService) 
        {
            _purchaseHistoryService = purchaseHistoryService;
        }
        public async Task<IEnumerable<PurchaseHistory>> Handle(GetPurQuery request, CancellationToken cancellationToken)
        {
            var pur = await _purchaseHistoryService.GetAll();
            return pur;
        }
    }
}
