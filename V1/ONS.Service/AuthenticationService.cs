using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.Core.Service.Interfaces;
using FMS.FrameWork;
using ONS.Core.Entities;
using ONS.Infrastructure;

namespace ONS.Service
{
    public class AuthenticationService:IAuthenticationService
    {
        public Result<Users> Login(string UserName, string password)
        {
            var result = new Result<Users>();
            try
            {
                string query = "select * from Users where UserName='" + UserName.ToUpper() + "' and Password='" + password.ToUpper() + "'";
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    result.HasError = true;
                    result.Message = "Invalid Email or Pssword";
                    return result;
                }

                result.Data = ConvertToEntity(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public Result<Users> GetByUserName(string UserName)
        {
            var result = new Result<Users>();
            try
            {
                string query = "select * from Users where UserName='" + UserName + "'";
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    result.HasError = true;
                    result.Message = "Invalid UserId";
                    return result;
                }

                result.Data = ConvertToEntity(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        private Users ConvertToEntity(DataRow row)
        {
            try
            {
                Users u = new Users();
                u.UserId = Int32.Parse(row["UserId"].ToString());
                u.UserName = row["UserName"].ToString();
                u.Password = row["Password"].ToString();
                u.UserType = row["UserType"].ToString();

                return u;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
