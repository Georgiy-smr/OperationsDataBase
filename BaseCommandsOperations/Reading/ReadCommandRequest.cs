using DataBaseOperationHelper.Abstractions;
using MediatR;
using StatusGeneric;

namespace DataBaseOperationHelper.BaseCommandsOperations.Reading;

public abstract record ReadCommandRequest<TEntity, TDto> :
    ReadCommand<TEntity>,
    IRequest<IStatusGeneric<IEnumerable<TDto>>>
    where TEntity : IEntity, new()
    where TDto : BaseDto;