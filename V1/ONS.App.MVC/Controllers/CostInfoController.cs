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


        public CostInfoController(IClientService clientservice, IUsersService userservice, ICollectionInfoService collectionservice, ICostLogService costservice)
        {
            _clientservice = clientservice;
            _userservice = userservice;
            _collectionservice = collectionservice;
            _costservice = costservice;
        }

        public ActionResult CreateCost()
        {
            return View();
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

        public ActionResult CostList()
        {
            try
            {
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

                return View(result);
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
              
                return View(result);
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
    
    }
}