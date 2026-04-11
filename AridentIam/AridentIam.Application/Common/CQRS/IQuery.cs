using MediatR;

namespace AridentIam.Application.Common.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}