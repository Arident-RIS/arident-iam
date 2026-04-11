using MediatR;

namespace AridentIam.Application.Common.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}