using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ONS.App.MVC.View_Model;
using ONS.Core.Entities;
using ONS.Core.Service.Interfaces;

namespace ONS.App.MVC.Controllers
{
    public class CostInfoController : Controller
    {
        private IClientService _clientservice;
        private IUsersService _userservice;
        private ICollectionInfoService _collectionservice;
        private ICostLogService _costservice;
        private IEmployeeService _employeeservice;



        public CostInfoController(IClientService clientservice, IUsersService userservice, ICollectionInfoService collectionservice, ICostLogService costservice, IEmployeeService employeeservice)
        {
            _clientservice = clientservice;
            _userservice = userservice;
            _collectionservice = collectionservice;
            _costservice = costservice;
            _employeeservice = employeeservice;
        }

        public ActionResult CreateCost()
        {
            try
            {
                var result = _employeeservice.GetAll();

           
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
        public ActionResult CreateCost(CostLog costLog)
        {
            try
            {
                var result = _costservice.Save(costLog);
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }

                return RedirectToAction("CostList");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult CostList(int id=0)
        {
            try
            {
                if (id != 0)
                    ViewBag.msg = 1;

                var result = _costservice.GetAll();
                double totalbalance = 0;
               
                foreach (var p in result.Data)
                {
                    totalbalance += p.Amount;
                    
                }

                ViewBag.totalbalance = (totalbalance);
                
                ViewBag.totalbalance1 = NumberToWord.NumberToWords((int)totalbalance);
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }
                CostListModel cost=new CostListModel();
                cost.CostLogs = result.Data;
                cost.Employees = _employeeservice.GetAll().Data;
                return View(cost);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
           
        }

        [HttpPost]
        public ActionResult CostList(string key1, string key2)
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
                    else if (key1.Contains("Month"))
                    {
                        key = key2.Substring(2);
                    }
                    else if (key2.Contains("Year"))
                    {
                        key = "-" + key1.Substring(0, 3);
                    }
                }
                else if (key1.Contains("Employee Name"))
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
                var result = _costservice.GetAll(key);

                ViewBag.totalclient = result.Data.Count;
                double totalbalance = 0;
              
                foreach (var p in result.Data)
                {
                    totalbalance += p.Amount;
                }

                ViewBag.totalbalance = (totalbalance);
              
                ViewBag.totalbalance1 = NumberToWord.NumberToWords((int)totalbalance);

                CostListModel cost = new CostListModel();
                cost.CostLogs = result.Data;
                cost.Employees = _employeeservice.GetAll().Data;
                return View(cost);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        public ActionResult CostDetails(int id)
        {
            try
            {
                var result = _costservice.GetById(id);
            
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
        public ActionResult PayAll(FormCollection formCollection)
        {
            try
            {
                string[] ids = formCollection["EmployeeId"].Split(new char[] { ',' });
                string[] ids2 = formCollection["Amount"].Split(new char[] { ',' });
                foreach (string id in ids)
                {
                    var i = Int32.Parse(id);
                    var clients = _employeeservice.GetById(i);
                    CostLog costLog = new CostLog();
                    costLog.CostName = "Salary";
                    costLog.EmployeeName = clients.Data.EmployeeName;
                    if (ids2[0].Equals(""))
                    {
                        if (clients.Data.Due != 0)
                        {
                            costLog.Amount = clients.Data.Due;
                            clients.Data.Due = 0;
                        }
                        else
                        {
                            costLog.Amount = clients.Data.Salary;

                        }
                      

                    }

                    else
                    {
                        costLog.Amount = double.Parse(formCollection["Amount"]);
                        clients.Data.Due = clients.Data.Due - double.Parse(formCollection["Amount"]);
                    }


                    _costservice.Save(costLog);
                    _employeeservice.DueSet(clients.Data);
                    if (clients.HasError)
                    {
                        ViewBag.Message = clients.Message;
                        return Content("Problem is : " + clients.Message);
                    }
                }


            }
            catch (Exception)
            {

                return RedirectToAction("CostList", "CostInfo", new { id = 1 });
            }
            return RedirectToAction("CostList", "CostInfo");
        }
    
    }
}