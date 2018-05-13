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
    public class ClientViewController : Controller
    {
      
        private IClientService _clientservice;
        private IUsersService _userservice;
        private ICollectionInfoService _collectionservice;


        public ClientViewController(IClientService clientservice, IUsersService userservice, ICollectionInfoService collectionservice)
        {
            _clientservice = clientservice;
            _userservice = userservice;
            _collectionservice = collectionservice;
        }

        public ActionResult Details()
        {
            try
            {
                var result = _clientservice.GetById(HttpUtil.CurrentUser.UserId);

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
        public ActionResult Details(Clients clients)
        {
            try
            {
                var result = _clientservice.Save(clients);
                var user = new Users();
                user.UserName = clients.UserName;
                user.Password = clients.Password;
                user.UserType = clients.UserType;
                var result2 = _userservice.Save(user);
                if (result2.HasError)
                {
                    ViewBag.Message = result2.Message;
                    return Content("Problem is : " + result2.Message);
                }
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }
                ViewBag.msg = 1;
                return View(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult PaymentHistory()
        {
            try
            {
                var result2 = _collectionservice.GetAll();
                var result = _collectionservice.GetAll().Data.Where(d=>d.UserId==HttpUtil.CurrentUser.UserId).ToList();
               
                double totalbalance = 0;
                foreach (var p in result)
                {
                    totalbalance += p.Amount;

                }
                ViewBag.totalbalance = totalbalance;
                if (result2.HasError)
                {
                    ViewBag.Message = result2.Message;
                    return Content("Problem is : " + result2.Message);
                }
                return View(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

         [HttpPost]
        public ActionResult PaymentHistory(Searching searching)
        {
            try
            {
                string key = "";
                if (searching.Month != null && searching.Year != null)
                {
                    if (!searching.Month.Contains("Month") && !searching.Year.Contains("Year"))
                    {
                        key = searching.Month.Substring(0, 3) + "-" + searching.Year.Substring(2);

                    }
                    else if (searching.Month.Contains("Month"))
                    {
                        key = searching.Year.Substring(2);
                    }
                    else if (searching.Year.Contains("Year"))
                    {
                        key = "-"+searching.Month.Substring(0, 3);
                    }
                }
                else if (searching.Key != null)
                {
                    key = searching.Key;
                }
                else
                {
                    key = " ";
                }
                var result2 = _collectionservice.GetAll();
                var result = _collectionservice.GetAll(key).Data.Where(d => d.UserId == HttpUtil.CurrentUser.UserId).ToList();
                double totalbalance = 0;
              
                foreach (var p in result)
                {
                    totalbalance += p.Amount;

                }
                ViewBag.totalbalance = totalbalance;
                if (result2.HasError)
                {
                    ViewBag.Message = result2.Message;
                    return Content("Problem is : " + result2.Message);
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