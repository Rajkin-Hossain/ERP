using MediatR;

namespace ERP.Shared.Application.Interfaces;

/// <summary>
/// Marker interface for commands that require transactional consistency.
/// </summary>
/// <typeparam name="TResponse">The type of response from the command.</typeparam>
public interface IBaseCommand<out TResponse> : IRequest<TResponse>
{
}
