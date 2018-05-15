using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.FrameWork;
using ONS.Core.Entities;

namespace ONS.Core.Service.Interfaces
{
    public interface IEmployeeService:IService<Employees>
    {
        Result<Employees> DueSet(Employees Employees);
        Result<Clients> MonthDue(int id);

    }
}
