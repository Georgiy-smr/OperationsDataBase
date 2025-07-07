using System.Linq.Expressions;
using DataBaseOperationHelper.Abstractions;

namespace DataBaseOperationHelper.BaseCommandsOperations.Reading;

public abstract record ReadCommand<T>(
    bool Tracked = true,
    bool OrderByDesc = true,
    bool AsSplitQuery = false) where T : IEntity, new()
{
    public required IEnumerable<Expression<Func<T, bool>>>? Filters { get; init; }
    public required IEnumerable<Expression<Func<T, object>>>? Includes { get; init; }
    public Expression<Func<T, object>> OrderBy { get; init; } = x => x.Id;
    public required int Size { get; init; }
    public required int ZeroStart { get; init; }
}