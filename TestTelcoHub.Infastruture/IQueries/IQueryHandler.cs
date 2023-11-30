using MediatR;

namespace TestTelcoHub.Infastruture.IQueries
{
    public interface IQueryHandler<in TQuery, TResult>:
        IRequestHandler<TQuery,TResult> where TQuery : IQuery<TResult>
    {
    }
}
