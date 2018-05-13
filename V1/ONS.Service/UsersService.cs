using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMS.FrameWork;
using ONS.Core.Entities;
using ONS.Core.Service.Interfaces;
using ONS.Infrastructure;

namespace ONS.Service
{
   public class UsersService:IUsersService
    {
        
       public Result<Users> Save(Users Users)
        {
            var result = new Result<Users>();
            try
            {
                string query = "select * from Users where UserName='" + Users.UserName.ToUpper()+"'";
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    Users.UserId = GetId();
                   
                    query = "insert into Users values(" + Users.UserId + ",'" + Users.UserName.ToUpper() + "','" + Users.Password.ToUpper() + "','" + Users.UserType.ToUpper() + "')";
                }
                else
                {
                    query = "update Users set Password='" + Users.Password.ToUpper() + "', UserName='" + Users.UserName.ToUpper() + "' where UserName='" + Users.UserName.ToUpper() + "'";

                }

                //if (!IsValid(Users, result))
                //{
                //    return result;
                //}

                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

                if (result.HasError)
                    result.Message = "Something Went Wrong";
                else
                {
                    result.Data = Users;
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }
     
       private int GetId()
        {
            string query = "select * from Users order by UserId desc";
            var dt = DataAccess.GetDataTable(query);
            int UserId = 1;

            if (dt != null && dt.Rows.Count != 0)
                UserId = Int32.Parse(dt.Rows[0]["UserId"].ToString()) + 1;

            return UserId;
        }
     
       public List<Users> GetAll()
        {
            var result = new List<Users>();
            try
            {
                string query = "select * from Users";
                var dt = DataAccess.GetDataTable(query);

                if (dt != null && dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Users u = ConvertToEntity(dt.Rows[i]);
                        result.Add(u);
                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

       public Result<Users> GetById(int UserId)
        {
            var result = new Result<Users>();
            try
            {
                string query = "select * from Users where UserId=" + UserId;
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
 
       public Result<List<Users>> GetAll(string key = "")
        {
            var result = new Result<List<Users>>() { Data = new List<Users>() };
            try
            {
                key = key.ToUpper();
                string query = "select * from Users ";
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " where UserName like '%" + key + "%' ";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR UserType like '%" + key + "%'";
                }
                
                var dt = DataAccess.GetDataTable(query);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    result.Data.Add(ConvertToEntity(dt.Rows[i]));
                }

            }
            catch (Exception e)
            {
                result.HasError = true;
                result.Message = e.Message;
            }
            return result;
        }

       public bool Delete(int UserId)
        {
            var result = new Result<Users>();
            try
            {
                string query = "delete from Users where UserId=" + UserId;
                return DataAccess.ExecuteQuery(query) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

       public bool IsValid(Users obj, Result<Users> result)
        {
            if (!ValidationHelper.IsStringValid(obj.UserName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Frist Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.UserType))
            {
                result.HasError = true;
                result.Message = "InvalUserId Last Name";
                return false;
            }
           

            return true;
        }

       private Users ConvertToEntity(DataRow row)
        {
            try
            {
                Users u = new Users();
                u.UserId = Int32.Parse(row["UserId"].ToString());
                u.UserName = row["UserName"].ToString().Substring(0, 1).ToUpper() + row["UserName"].ToString().Substring(1).ToLower();
                u.UserType = row["UserType"].ToString().Substring(0, 1).ToUpper() + row["UserType"].ToString().Substring(1).ToLower();
                u.Password = row["Password"].ToString();
                
                return u;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
