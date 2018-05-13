using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.FrameWork;
using ONS.Core.Entities;

namespace FMS.Core.Service.Interfaces
{
   public interface IAuthenticationService
   {
       Result<Users> Login(string username, string password);
       Result<Users> GetByUserName(string username);

   }
}
