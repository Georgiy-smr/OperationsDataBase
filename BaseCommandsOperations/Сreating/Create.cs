using DataBaseOperationHelper.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseOperationHelper.BaseCommandsOperations.Сreating
{
    public abstract record Create<T>(T Сreated) : OperationRequestFromDataBase where T : BaseDto;
}
