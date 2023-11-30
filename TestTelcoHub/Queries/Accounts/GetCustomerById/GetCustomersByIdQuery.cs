using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Queries.Accounts.GetCustomerById
{
    public class GetCustomersByIdQuery : IQuery<Customer>
    {
        public string CustomerId { get; set; }
        public GetCustomersByIdQuery(string customerId) { CustomerId = customerId; }

    }
}
