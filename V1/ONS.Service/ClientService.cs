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
   public class ClientService:IClientService
    {
        public Result<Clients> Save(Clients Clients)
        {
            var result = new Result<Clients>();
            try
            {
                string query = "select * from Clients where ClientId=" + Clients.ClientId;
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    Clients.ClientId = GetId();
                    var joindate = DateTime.Now.ToString(string.Format("dd/MMM/yyyy"));
                    query = "insert into Clients values(" + Clients.ClientId + ",'" + Clients.ClientName.ToUpper() + "','" + Clients.ClientEmail.ToUpper() + "','" + Clients.Address.ToUpper() + "','" + Clients.PhoneNumber.ToUpper() + "','" + Clients.Package.ToUpper() + "'," + Clients.MonthlyBill + "," + Clients.ConnectionFee + "," + Clients.Others + "," + Clients.RouterFee + "," + Clients.Pay + ",'" + joindate + "'," + Clients.Due + ",'" + Clients.UserName.ToUpper() + "','" + Clients.Password.ToUpper() + "','" + Clients.UserType.ToUpper() + "'," + 0 + "," + 0 + ")";
                }
                else
                {

                    query = "update Clients set ClientName='" + Clients.ClientName.ToUpper() + "',PhoneNumber='" + Clients.PhoneNumber.ToUpper() + "',UserName='" + Clients.UserName.ToUpper() + "',Password='" + Clients.Password.ToUpper() + "',Address='" + Clients.Address.ToUpper() + "',ClientEmail='" + Clients.ClientEmail.ToUpper() + "',Package='" + Clients.Package.ToUpper() + "',MonthlyBill=" + Clients.MonthlyBill + ",Pay=" + Clients.Pay + " " + ",Due=" + Clients.Due + " " +
                            " where ClientId=" + Clients.ClientId;


                }


                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

                if (result.HasError)
                    result.Message = "Something Went Wrong";
                else
                {
                    result.Data = Clients;
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
            string query = "select * from Clients order by ClientId desc";
            var dt = DataAccess.GetDataTable(query);
            int ClientId = 1;

            if (dt != null && dt.Rows.Count != 0)
                ClientId = Int32.Parse(dt.Rows[0]["ClientId"].ToString())+1;

            return ClientId;
        }

        public Result<Clients> GetById(int ClientId)
        {
            var result = new Result<Clients>();
            try
            {
                string query = "select * from Clients where ClientId=" + ClientId;
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    result.HasError = true;
                    result.Message = "Invalid ClientId";
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

        public Result<List<Clients>> GetAll(string key = "")
        {
            var result = new Result<List<Clients>>() { Data = new List<Clients>() };
            try
            {
                key = key.ToUpper();
                string query = "select * from Clients ";
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " where ClientName like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR ClientEmail like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR Address like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR  PhoneNumber like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                   
                    query += " OR  JoinDate like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {

                    query += " OR  UserName like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR  Package like '%" + key + "%'";
                }
                if (ValidationHelper.IsIntValid(key))
                {
                    var m = Int32.Parse(key);
                    query += " OR  MonthlyBill like " + m + "";
                }
                query += " order by ClientName asc  ";
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

        public bool Delete(int ClientId)
        {
            var result = new Result<Clients>();
            try
            {
                string query = "delete from Clients where ClientId=" + ClientId;
                return DataAccess.ExecuteQuery(query) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Result<Clients> Hide(int id)
        {
            var result = new Result<Clients>();
            try
            {
                string query = "select * from Clients where ClientId=" + id;
               
                    query = "update Clients set IsValid=" +1+ " where ClientId=" + id;


                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

               
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public Result<Clients> MonthDue(int id)
        {
            var result = new Result<Clients>();
            try
            {


                string query = "update Clients set IsNewMonth=" + 1 + " where ClientId=" + id;


                result.HasError = DataAccess.ExecuteQuery(query) <= 0;


            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public Result<Clients> Payment(Clients clients)
        {
            var result = new Result<Clients>();
            try
            {
                string query = "select * from Clients where ClientId=" + clients.ClientId;

                query = "update Clients set Due=" + clients.Due + ", Pay=" + clients.Pay + " where ClientId=" + clients.ClientId;


                result.HasError = DataAccess.ExecuteQuery(query) <= 0;


            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public bool IsValid(Clients obj, Result<Clients> result)
        {
            if (!ValidationHelper.IsStringValid(obj.ClientEmail))
            {
                result.HasError = true;
                result.Message = "InvalUserId Frist Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.ClientName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Last Name";
                return false;
            }


            return true;
        }

        private Clients ConvertToEntity(DataRow row)
        {
            try
            {
                Clients u = new Clients();
                u.ClientId = Int32.Parse(row["ClientId"].ToString());
                u.MonthlyBill = Int32.Parse(row["MonthlyBill"].ToString());
                u.Pay = Int32.Parse(row["Pay"].ToString());
                u.Due = Int32.Parse(row["Due"].ToString());
                u.RouterFee = Int32.Parse(row["RouterFee"].ToString());
                u.ConnectionFee = Int32.Parse(row["ConnectionFee"].ToString());
                u.Others = Int32.Parse(row["Others"].ToString());
                u.IsValid = Int32.Parse(row["IsValid"].ToString());
                u.IsNewMonth = Int32.Parse(row["IsNewMonth"].ToString());
                u.ClientEmail = row["ClientEmail"].ToString().ToLower();
                u.ClientName = row["ClientName"].ToString().Substring(0, 1).ToUpper() + row["ClientName"].ToString().Substring(1).ToLower();
                u.UserName = row["UserName"].ToString().Substring(0, 1).ToUpper() + row["UserName"].ToString().Substring(1).ToLower();
                u.Password = row["Password"].ToString().Substring(0, 1).ToUpper() + row["Password"].ToString().Substring(1).ToLower();
                u.UserType = row["UserType"].ToString().Substring(0, 1).ToUpper() + row["UserType"].ToString().Substring(1).ToLower();
                u.Address = row["Address"].ToString().Substring(0, 1).ToUpper() + row["Address"].ToString().Substring(1).ToLower();
                u.Package = row["Package"].ToString().Substring(0, 1).ToUpper() + row["Package"].ToString().Substring(1).ToLower();
                u.PhoneNumber = row["PhoneNumber"].ToString();
                u.JoinDate = Convert.ToDateTime(row["JoinDate"]);

               

                return u;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
