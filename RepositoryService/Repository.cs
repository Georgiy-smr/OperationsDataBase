using DataBaseOperationHelper.Abstractions;
using MediatR;
using StatusGeneric;

namespace DataBaseOperationHelper.RepositoryService;

public class Repository : IRepository
{
    private readonly IMediator _mediator;
    public Repository(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task<IStatusGeneric<IEnumerable<TDto>>> GetItemsAsync<TDto>(
        IRequest<IStatusGeneric<IEnumerable<TDto>>> command, CancellationToken token = default)
        where TDto : BaseDto
    {
        return _mediator.Send(command, token);
    }

    public Task<IStatusGeneric> DataBaseOperationAsync(
        OperationRequestFromDataBase requestCommand,
        CancellationToken token = default)
    {
        return _mediator.Send(requestCommand, token);
    }

}