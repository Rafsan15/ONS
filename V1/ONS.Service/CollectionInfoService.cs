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
    public class CollectionInfoService:ICollectionInfoService
    {
        public Result<CollectionInfo> Save(CollectionInfo CollectionInfo)
        {
            var result = new Result<CollectionInfo>();
            try
            {
                string query = "select * from CollectionInfo where CollectionId=" + CollectionInfo.CollectionId;
               
                    CollectionInfo.CollectionId = GetId();
                    if (CollectionInfo.UserId==0)
                         CollectionInfo.UserId = GetId2();

                    var CollectionDate = DateTime.Now.ToString(string.Format("dd/MMM/yyyy"));

                    query = "insert into CollectionInfo values(" + CollectionInfo.CollectionId + "," + CollectionInfo.UserId + "," + CollectionInfo.Amount + ",'" + CollectionDate + "','" + CollectionInfo.UserName.ToUpper() + "')";
                
               

                //if (!IsValid(CollectionInfo, result))
                //{
                //    return result;
                //}

                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

                if (result.HasError)
                    result.Message = "Something Went Wrong";
                else
                {
                    result.Data = CollectionInfo;
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
            string query = "select * from CollectionInfo order by CollectionId desc";
            var dt = DataAccess.GetDataTable(query);
            int CollectionId = 1;

            if (dt != null && dt.Rows.Count != 0)
                CollectionId = Int32.Parse(dt.Rows[0]["CollectionId"].ToString())+1;

            return CollectionId;
        }

        private int GetId2()
        {
            string query = "select * from Clients order by ClientId desc";
            var dt = DataAccess.GetDataTable(query);
            int UserId = 1;

            if (dt != null && dt.Rows.Count != 0)
                UserId = Int32.Parse(dt.Rows[0]["ClientId"].ToString());

            return UserId;
        }

        public Result<List<CollectionInfo>> GetAllById(int CollectionId)
        {
            var result = new Result<List<CollectionInfo>>() { Data = new List<CollectionInfo>() };

            try
            {
                string query = "select * from CollectionInfo where UserId=" + CollectionId;
                var dt = DataAccess.GetDataTable(query);

                if (dt != null && dt.Rows.Count != 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        result.Data.Add(ConvertToEntity(dt.Rows[i]));

                    }
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        public Result<CollectionInfo> GetById(int CollectionId)
        {
            var result = new Result<CollectionInfo>();
            try
            {
                string query = "select * from CollectionInfo where CollectionId=" + CollectionId;
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

        public Result<List<CollectionInfo>> GetAll(string key = "")
        {
            var result = new Result<List<CollectionInfo>>() { Data = new List<CollectionInfo>() };
            try
            {
                key = key.ToUpper();
                string query = "select * from CollectionInfo  ";
                if (ValidationHelper.IsStringValid(key))
                {
                   
                    query += " where CollectionDate like '%" + key + "%'";
                }

                if (ValidationHelper.IsStringValid(key))
                {
                    query += " Or UserName like '%" + key + "%'";
                }
                if (ValidationHelper.IsIntValid(key))
                {
                    var m = Int32.Parse(key);
                    query += " Or UserId like " + m + " ";
                }
                if (ValidationHelper.IsIntValid(key))
                {
                    var m = Int32.Parse(key);

                    query += " Or Amount like " + m + "";
                }

                query += " order by CollectionDate,Amount,UserName asc  ";

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
            var result = new Result<CollectionInfo>();
            try
            {
                string query = "delete from CollectionInfo where UserId=" + UserId;
                return DataAccess.ExecuteQuery(query) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsValid(CollectionInfo obj, Result<CollectionInfo> result)
        {
            


            return true;
        }

        private CollectionInfo ConvertToEntity(DataRow row)
        {
            try
            {
                CollectionInfo u = new CollectionInfo();
                u.UserId = Int32.Parse(row["UserId"].ToString());
                u.CollectionId = Int32.Parse(row["CollectionId"].ToString());
                u.Amount = Int32.Parse(row["Amount"].ToString());
                u.CollectionDate = Convert.ToDateTime(row["CollectionDate"]);
                u.UserName = row["UserName"].ToString().Substring(0, 1).ToUpper() + row["UserName"].ToString().Substring(1).ToLower();


                return u;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
