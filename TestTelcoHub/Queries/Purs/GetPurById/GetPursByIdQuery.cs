using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Queries.Purs.GetPurById
{
    public class GetPursByIdQuery :IQuery<PurchaseHistory>
    {
        public string SubscriptionId { get; set; }
        public GetPursByIdQuery(string subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }
}
