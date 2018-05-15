using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ONS.App.FrameWork;
using ONS.Core.Entities;
using ONS.Core.Service.Interfaces;

namespace ONS.App.MVC.Controllers
{
    public class AdminController : Controller
    {
        private IClientService _clientservice;
        private IUsersService _userservice;
        private ICollectionInfoService _collectionservice;
        private IEmployeeService _employeeservice;


        public AdminController(IClientService clientservice, IUsersService userservice, ICollectionInfoService collectionservice,IEmployeeService employeeservice)
        {
            _clientservice = clientservice;
            _userservice = userservice;
            _collectionservice = collectionservice;
            _employeeservice = employeeservice;
        }

        public ActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(Employees employees)
        {
            try
            {
                var result = _employeeservice.Save(employees);
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }

                return RedirectToAction("EmployeeList");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }

        private double getmonthlydue(Employees employees)
        {
            var due = employees.Due;
            var total = employees.Due + employees.Salary;
            if (DateTime.Today.Day == 15)
            {
                if (total - employees.Due <= employees.Salary)
                {
                    due += employees.Salary;

                }
            }
            return due;


        }

        public ActionResult EmployeeList()
        {

            try
            {
                var result = _employeeservice.GetAll();
                foreach (var p in result.Data)
                {
                    if (p.IsNewMonth == 0)
                    {
                        p.Due = getmonthlydue(p);
                        p.IsNewMonth = 1;
                        _employeeservice.DueSet(p);
                        _employeeservice.MonthDue(p.EmployeeId);

                    }
                    if (DateTime.Today.Day == DateTime.Today.Day + 1)
                    {
                        p.IsNewMonth = 0;
                        _employeeservice.MonthDue(p.EmployeeId);
                    }


                }
                ViewBag.totalemp = result.Data.Count;
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult EmployeeList(string key1, string key2)
        {

            try
            {
                string key = "";
                if (key1 != null && key2 != null)
                {
                    if (!key1.Contains("Month") && !key2.Contains("Year"))
                    {
                        key = key1.Substring(0, 3) + "-" + key2.Substring(2);

                    }
                    else if (key1.Contains("Month") && key2.Contains("Year"))
                    {
                        key = " ";
                    }
                    else if (key1.Contains("Month"))
                    {
                        key = key2.Substring(2);
                    }
                    else if (key2.Contains("Year"))
                    {
                        key = "-" + key1.Substring(0, 3);
                    }
                }


                else if (key1.Contains("Address"))
                {
                    key = " ";

                }
                else if (key1.Contains("Designation"))
                {
                    key = " ";

                }
                else if (key2 == null)
                {
                    key = key1;
                }
                else
                {
                    key = " ";
                }
                var result = _employeeservice.GetAll(key);
                foreach (var p in result.Data)
                {
                    if (p.IsNewMonth == 0)
                    {
                        p.Due = getmonthlydue(p);
                        p.IsNewMonth = 1;
                        _employeeservice.DueSet(p);
                        _employeeservice.MonthDue(p.EmployeeId);

                    }
                    if (DateTime.Today.Day == DateTime.Today.Day + 1)
                    {
                        p.IsNewMonth = 0;
                        _employeeservice.MonthDue(p.EmployeeId);
                    }


                }
                ViewBag.totalemp = result.Data.Count;
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeeDetail(int id)
        {

            try
            {
                var result = _employeeservice.GetById(id);
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }

                return View(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult EmployeeDelete(int id)
        {

            try
            {
                var result = _employeeservice.Delete(id);
               

                return RedirectToAction("EmployeeList");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult Propicup(HttpPostedFileBase file,int id)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                    Server.MapPath("~/DP"), pic);
                // file is uploaded
                file.SaveAs(path);
                var user = _employeeservice.GetById(id);
                user.Data.Propic = pic;
                _employeeservice.Save(user.Data);

            }
            
            return RedirectToAction("EmployeeList", "Admin");
        }

    }
}