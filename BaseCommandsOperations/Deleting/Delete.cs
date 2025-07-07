using DataBaseOperationHelper.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseOperationHelper.BaseCommandsOperations.Deleting
{
    public abstract record Delete(int IdEntity) : OperationRequestFromDataBase();
}
