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
    public class EmployeeService:IEmployeeService
    {
        public Result<Employees> Save(Employees Employees)
        {
            var result = new Result<Employees>();
            try
            {
                string query = "select * from Employees where EmployeeId=" + Employees.EmployeeId;
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    Employees.EmployeeId = GetId();
                    var dob = Employees.DOB.ToString(string.Format("dd/MMM/yyyy"));
                    var joindate = DateTime.Now.ToString(string.Format("dd/MMM/yyyy"));

                    query = "insert into Employees values(" + Employees.EmployeeId + ",'" + Employees.EmployeeName.ToUpper() + "','" + Employees.FatherName.ToUpper() + "','" + Employees.MotherName.ToUpper() + "','" + Employees.Email + "','" + Employees.PermanentAddress.ToUpper() + "','" + Employees.PresentAddress.ToUpper() + "','" + Employees.Nid + "','" + dob + "','" + joindate + "','" + Employees.Mobile.ToUpper() + "','" + Employees.Designation.ToUpper() + "','" + Employees.Propic + "'," + Employees.Salary + "," + 0 + "," + 0 + ")";
                }
                else
                {
                    query = "update Employees set EmployeeName='" + Employees.EmployeeName.ToUpper() + "', FatherName='" + Employees.FatherName.ToUpper() + "', MotherName='" + Employees.MotherName.ToUpper() + "', Nid='" + Employees.Nid + "', Designation='" + Employees.Designation.ToUpper() + "', Email='" + Employees.Email + "', Propic='" + Employees.Propic + "', Salary=" + Employees.Salary + ",PresentAddress='" + Employees.PresentAddress.ToUpper() + "', PermanentAddress='" + Employees.PermanentAddress.ToUpper() + "', Mobile='" + Employees.Mobile.ToUpper() + "' where EmployeeId=" + Employees.EmployeeId;

                }

                //if (!IsValid(Employees, result))
                //{
                //    return result;
                //}

                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

                if (result.HasError)
                    result.Message = "Something Went Wrong";
                else
                {
                    result.Data = Employees;
                }
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public Result<Employees> DueSet(Employees Employees)
        {
            var result = new Result<Employees>();
            try
            {
                string query;
              
                
                    query = "update Employees set Due=" + Employees.Due+ " where EmployeeId=" + Employees.EmployeeId;

                result.HasError = DataAccess.ExecuteQuery(query) <= 0;

                if (result.HasError)
                    result.Message = "Something Went Wrong";
                else
                {
                    result.Data = Employees;
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
            string query = "select * from Employees order by EmployeeId desc";
            var dt = DataAccess.GetDataTable(query);
            int EmployeeId = 1;

            if (dt != null && dt.Rows.Count != 0)
                EmployeeId = Int32.Parse(dt.Rows[0]["EmployeeId"].ToString()) + 1;

            return EmployeeId;
        }

        public Result<Employees> GetById(int EmployeeId)
        {
            var result = new Result<Employees>();
            try
            {
                string query = "select * from Employees where EmployeeId=" + EmployeeId;
                var dt = DataAccess.GetDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    result.HasError = true;
                    result.Message = "Invalid EmployeeId";
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

        public Result<List<Employees>> GetAll(string key = "")
        {
            var result = new Result<List<Employees>>() { Data = new List<Employees>() };
            try
            {
                key = key.ToUpper();
                string query = "select * from Employees ";
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " where EmployeeName like '%" + key + "%' ";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR Designation like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR JoinDate like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR DOB like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR PermanentAddress like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR PresentAddress like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR Mobile like '%" + key + "%'";
                }
                if (ValidationHelper.IsStringValid(key))
                {
                    query += " OR Email like '%" + key + "%'";
                }
                if (ValidationHelper.IsIntValid(key))
                {
                    var m = Int32.Parse(key);
                    query += " OR Salary like " + key + "";
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

        public bool Delete(int EmployeeId)
        {
            var result = new Result<Employees>();
            try
            {
                string query = "delete from Employees where EmployeeId=" + EmployeeId;
                return DataAccess.ExecuteQuery(query) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Result<Clients> MonthDue(int id)
        {
            var result = new Result<Clients>();
            try
            {


                string query = "update Employees set IsNewMonth=" + 1 + " where EmployeeId=" + id;


                result.HasError = DataAccess.ExecuteQuery(query) <= 0;


            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public bool IsValid(Employees obj, Result<Employees> result)
        {
            if (!ValidationHelper.IsStringValid(obj.EmployeeName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Employee Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.FatherName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Father Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.MotherName))
            {
                result.HasError = true;
                result.Message = "InvalUserId Mother Name";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.PermanentAddress))
            {
                result.HasError = true;
                result.Message = "InvalUserId Permanent Address";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.PresentAddress))
            {
                result.HasError = true;
                result.Message = "InvalUserId Present Address";
                return false;
            }
          
            if (!ValidationHelper.IsStringValid(obj.Mobile))
            {
                result.HasError = true;
                result.Message = "InvalUserId Mobile";
                return false;
            }
            if (!ValidationHelper.IsStringValid(obj.Designation))
            {
                result.HasError = true;
                result.Message = "InvalUserId Designation";
                return false;
            }
            


            return true;
        }

        private Employees ConvertToEntity(DataRow row)
        {
            try
            {
                Employees u = new Employees();
                u.EmployeeId = Int32.Parse(row["EmployeeId"].ToString());
                u.IsNewMonth = Int32.Parse(row["IsNewMonth"].ToString());
                u.Salary = Int32.Parse(row["Salary"].ToString());
                u.Due = Int32.Parse(row["Due"].ToString());
                u.EmployeeName = row["EmployeeName"].ToString().Substring(0, 1).ToUpper() + row["EmployeeName"].ToString().Substring(1).ToLower();
                u.FatherName = row["FatherName"].ToString().Substring(0, 1).ToUpper() + row["FatherName"].ToString().Substring(1).ToLower();
                u.MotherName = row["MotherName"].ToString().Substring(0, 1).ToUpper() + row["MotherName"].ToString().Substring(1).ToLower();
                u.Email = row["Email"].ToString();
                u.PermanentAddress = row["PermanentAddress"].ToString().Substring(0, 1).ToUpper() + row["PermanentAddress"].ToString().Substring(1).ToLower();
                u.PresentAddress = row["PresentAddress"].ToString().Substring(0, 1).ToUpper() + row["PresentAddress"].ToString().Substring(1).ToLower();
                u.Nid = row["Nid"].ToString();
                u.DOB = Convert.ToDateTime(row["DOB"]);
                u.JoinDate = Convert.ToDateTime(row["JoinDate"]);
                u.Mobile = row["Mobile"].ToString().Substring(0, 1).ToUpper() + row["Mobile"].ToString().Substring(1).ToLower();
                u.Designation = row["Designation"].ToString().Substring(0, 1).ToUpper() + row["Designation"].ToString().Substring(1).ToLower();
                u.Propic = row["Propic"].ToString();

                return u;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
