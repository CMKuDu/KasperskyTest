using MediatR;

namespace TestTelcoHub.Infastruture.IQueries
{
    public interface IQuery<out TResult>: IRequest<TResult>
    {
    }
}
