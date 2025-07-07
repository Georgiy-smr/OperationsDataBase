using DataBaseOperationHelper.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseOperationHelper.BaseCommandsOperations.Updating
{
    public record Update<T>(T Modified) : OperationRequestFromDataBase where T : BaseDto;
}
