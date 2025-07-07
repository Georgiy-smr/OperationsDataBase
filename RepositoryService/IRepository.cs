using DataBaseOperationHelper.Abstractions;
using MediatR;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseOperationHelper.RepositoryService
{
    public interface IRepository
    {
        Task<IStatusGeneric> DataBaseOperationAsync(
            OperationRequestFromDataBase requestCommand,
            CancellationToken token = default);

        Task<IStatusGeneric<IEnumerable<TDto>>> GetItemsAsync<TDto>(
            IRequest<IStatusGeneric<IEnumerable<TDto>>> command, CancellationToken token = default)
            where TDto : BaseDto;
    }
}
