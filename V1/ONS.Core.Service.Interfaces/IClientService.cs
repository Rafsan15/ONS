using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.FrameWork;
using ONS.Core.Entities;

namespace ONS.Core.Service.Interfaces
{
    public interface IClientService:IService<Clients>
    {
        Result<Clients> Hide(int id);
        Result<Clients> Payment(Clients clients);
    }
}
