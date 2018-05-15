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
  public  class CostLogService:ICostLogService
    {
        public Result<CostLog> Save(CostLog CostLog)
        {
            var result = new Result<CostLog>();
            try
            {
                string query = "select * from CostLog where CostId=" + CostLog.CostId;
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    CostLog.CostId = GetId();
                    var costDate = DateTime.Now.ToString(string.Format("dd/MMM/yyyy"));

                    query = "insert into CostLog values(" + CostLog.CostId + ",'" + CostLog.CostName.ToUpper() + "','" + costDate + "','" + CostLog.EmployeeName + "','" + CostLog.IssuedBy + "'," + CostLog.Amount + ")";
                }
                else
                {
                    query = "update CostLog set CostName='" + CostLog.CostName.ToUpper() + "', IssuedBy='" + CostLog.IssuedBy + "', EmployeeName='" + CostLog.EmployeeName + "', Amount=" + CostLog.Amount + " where CostId=" + CostLog.CostId;

                }

                //if (!IsValid(CostLog, result))
                //{
                //    return result;
                //}

                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

                if (result.HasError)
                    result.Message = "Something Went Wrong";
                else
                {
                    result.Data = CostLog;
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
            string query = "select * from CostLog order by CostId desc";
            var dt = DataAccess.GetDataTable(query);
            int CostId = 1;

            if (dt != null && dt.Rows.Count != 0)
                CostId = Int32.Parse(dt.Rows[0]["CostId"].ToString()) + 1;

            return CostId;
        }

        public Result<CostLog> GetById(int CostId)
        {
            var result = new Result<CostLog>();
            try
            {
                string query = "select * from CostLog where CostId=" + CostId;
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    result.HasError = true;
                    result.Message = "Invalid CostId";
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

        public Result<List<CostLog>> GetAll(string key = "")
        {
            var result = new Result<List<CostLog>>() { Data = new List<CostLog>() };
            try
            {
                string query = "select * from CostLog ";
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " where CostName like '%" + key.ToUpper() + "%' ";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR EmployeeName like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR IssuedBy like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR CostDate like '%" + key.ToUpper() + "%'";
                }
                if (ValidationHelper.IsIntValid(key))
                {
                    var m = Int32.Parse(key);
                    query += " OR Amount like " + key + "";
                }
                query += " order by CostId DESC ";
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

        public bool Delete(int CostId)
        {
            var result = new Result<CostLog>();
            try
            {
                string query = "delete from CostLog where CostId=" + CostId;
                return DataAccess.ExecuteQuery(query) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsValid(CostLog obj, Result<CostLog> result)
        {
            if (!ValidationHelper.IsStringValid(obj.CostName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Cost Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.IssuedBy))
            {
                result.HasError = true;
                result.Message = "InvalUserId IssuedBy Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.EmployeeName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Employee Name";
                return false;
            }


            return true;
        }

        private CostLog ConvertToEntity(DataRow row)
        {
            try
            {
                CostLog u = new CostLog();
                u.CostId = Int32.Parse(row["CostId"].ToString());
                u.Amount = Int32.Parse(row["Amount"].ToString());
                u.CostName = row["CostName"].ToString().Substring(0, 1).ToUpper() + row["CostName"].ToString().Substring(1).ToLower();
                u.EmployeeName = row["EmployeeName"].ToString();
                u.IssuedBy = row["IssuedBy"].ToString();
                u.CostDate = Convert.ToDateTime(row["CostDate"]);

                return u;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
