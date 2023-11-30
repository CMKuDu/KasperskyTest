using TestTelcoHub.Infastruture.IQueries;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Queries.Accounts.GetCustomerById
{
    public class GetCustomerByIdQueryHander : IQueryHandler<GetCustomersByIdQuery, Customer>
    {
        public Task<Customer> Handle(GetCustomersByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
