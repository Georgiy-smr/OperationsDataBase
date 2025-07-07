using MediatR;
using StatusGeneric;

namespace DataBaseOperationHelper.Abstractions;

public abstract record OperationRequestFromDataBase() : IRequest<IStatusGeneric>;