using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FMS.Core.Service.Interfaces;
using ONS.App.FrameWork;
using ONS.App.MVC.View_Model;
using ONS.Core.Entities;
using ONS.Core.Service.Interfaces;

namespace ONS.App.MVC.Controllers
{
    public class ClientInfoController : Controller
    {
       
        private IClientService _clientservice;
        private IUsersService _userservice;
        private ICollectionInfoService _collectionservice;


        public ClientInfoController(IClientService clientservice, IUsersService userservice, ICollectionInfoService collectionservice)
        {
            _clientservice = clientservice;
            _userservice = userservice;
            _collectionservice = collectionservice;
        }

        public ActionResult ClientForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ClientForm(Clients clients)
        {
            if (!ModelState.IsValid && clients.ClientId==0)
            {
                return View("ClientForm", clients);
            }
            try
            {
                
                var c = 0;
                if (clients.Pay == 0)
                {
                    c++;
                }

                CollectionInfo collection=new CollectionInfo();
                collection.Amount = clients.Pay;
                collection.UserName = clients.ClientName;
                if (clients.ClientId == 0)
                {
                    clients.Due = clients.MonthlyBill + clients.ConnectionFee + clients.RouterFee + clients.Others - clients.Pay;
                  
                }

                else
                {
                    if(clients.Due<clients.Pay)
                    {
                        ViewBag.msg = 2;
                        return RedirectToAction("ClientDetail", "ClientInfo",new{id=clients.ClientId,id2=2});
                    }
                    clients.Due = getdue(clients); 
                    collection.UserId = clients.ClientId;
                }
                clients.Pay = clients.Paid + clients.Pay;

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
                 if(c==0)
                       _collectionservice.Save(collection);
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }
              
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return RedirectToAction("ClientShowAll", "ClientInfo");
        }

        private double getmonthlydue(Clients clients)
        {
            var due = clients.Due;
            var total = clients.Due + clients.MonthlyBill;
            if (DateTime.Today.Day == clients.JoinDate.Day)
            {
                if (total - clients.Due <= clients.MonthlyBill && clients.Due==0)
                {
                    due += clients.MonthlyBill;

                }
            }
            return due;


        }

        private double getdue(Clients clients)
        {
            var due = clients.Due - clients.Pay;
            var total = clients.Due + clients.MonthlyBill;
            if (DateTime.Today.Day == 29)
            {
                if (total - clients.Due < clients.MonthlyBill)
                {
                    due += clients.MonthlyBill;

                }
            }
            return due;


        }
   
        public ActionResult ClientShowAll( int id=0)
        {
            try
            {
                
                //var a = HttpUtil.CurrentUser.UserId;
                if (id != 0)
                    ViewBag.msg = 1;
                var result = _clientservice.GetAll();
                var result2 = _clientservice.GetAll().Data.Where((d => d.IsValid == 0)).ToList();
               
                    foreach (var p in result2)
                      {
                          if (p.IsNewMonth == 0)
                          {
                              p.Due = getmonthlydue(p);
                              p.IsNewMonth = 1;
                              _clientservice.Save(p);
                              _clientservice.MonthDue(p.ClientId);
                             
                          }
                          if (DateTime.Today.Day == p.JoinDate.Day+1)
                          {
                              p.IsNewMonth = 0;
                              _clientservice.MonthDue(p.ClientId);
                          }
                       

                      }

                  
                double totalbalance = 0;
                double totaldue = 0;
               
                    foreach (var p in result2)
                    {
                        totalbalance += p.Pay;
                        totaldue += p.Due;
                    }


                   

                ViewBag.totalbalance = (totalbalance);
                ViewBag.totaldue = (totaldue);
                ViewBag.totalbalance1 = NumberToWord.NumberToWords((int)totalbalance);
                ViewBag.totaldue1 = NumberToWord.NumberToWords((int)totaldue);
                ViewBag.totalclient1 = result2.Count;

                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }
                
                return View(result2);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
           
        }

        [HttpPost]
        public ActionResult ClientShowAll( string key1,string key2 )
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
                        key =" ";
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
                else if (key1.Contains("BandWidth"))
                {
                    key = " ";

                }
                else if (key2 == null)
                {
                    key = key1;
                }
                else
                {
                    key =" ";
                }
                var result = _clientservice.GetAll(key).Data.Where((d => d.IsValid == 0)).ToList();
                foreach (var p in result)
                {
                    if (p.IsNewMonth == 0)
                    {
                        p.Due = getmonthlydue(p);
                        p.IsNewMonth = 1;
                        _clientservice.Save(p);
                        _clientservice.MonthDue(p.ClientId);
                       
                    }
                    if (DateTime.Today.Day == p.JoinDate.Day + 1)
                    {
                        p.IsNewMonth = 0;
                        _clientservice.MonthDue(p.ClientId);
                    }


                }
               
                ViewBag.totalclient1= result.Count;
                double totalbalance = 0;
                double totaldue = 0;
                foreach (var p in result)
                {
                    totalbalance += p.Pay;
                    totaldue += p.Due;
                }

                ViewBag.totalbalance = (totalbalance);
                ViewBag.totaldue = (totaldue);
                ViewBag.totalbalance1 = NumberToWord.NumberToWords((int)totalbalance);
                ViewBag.totaldue1 = NumberToWord.NumberToWords((int)totaldue);
                return View(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }
    
        public ActionResult ClientDetail(int id,int id2=0)
        {
            
            try
            {
                if (id2 != 0)
                {
                    ViewBag.msg = 2;
                }
                var result = _clientservice.GetById(id);
             
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


        public ActionResult ClientDelete(int id)
        {
          
            try
            {
                _clientservice.Hide(id);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return RedirectToAction("ClientShowAll", "ClientInfo");
        }

        public ActionResult CollectionLog()
        {
            try
            {
               
                var result = _collectionservice.GetAll();
                var result2 = _clientservice.GetAll().Data.Where(d=>d.IsValid==0).ToList();
                ViewBag.totalclient = result2.Count;
                double totalbalance = 0;
                foreach (var p in result.Data)
                {
                    totalbalance += p.Amount;
                   
                }

                ViewBag.totalbalance = totalbalance;
                ViewBag.totalbalance2 = NumberToWord.NumberToWords((int)totalbalance);
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
        public ActionResult CollectionLog(Searching searching)
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
                    else if (searching.Month.Contains("Month") && searching.Year.Contains("Year"))
                    {
                        key = " ";
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
                
                var result = _collectionservice.GetAll(key);
                var result2 = _clientservice.GetAll().Data.Where(d => d.IsValid == 0).ToList();
                ViewBag.totalclient = result2.Count;
                
                double totalbalance = 0;

                foreach (var p in result.Data)
                {
                    totalbalance += p.Amount;

                }

                ViewBag.totalbalance = totalbalance;
                ViewBag.totalbalance2 = NumberToWord.NumberToWords((int)totalbalance);
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

        public ActionResult AdminProfile()
        {
            try
            {
                var result = _userservice.GetById(HttpUtil.CurrentUser.UserId);
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
        public ActionResult AdminProfile(Users user)
        {
            try
            {
                user.UserId = HttpUtil.CurrentUser.UserId;
                user.UserType = "OWNER";
                var result = _userservice.Save(user);
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

        public ActionResult Pay(int id,int id2=0)
        {
            try
            {
                if (id2 != 0)
                {
                    ViewBag.msg = 2;
                }
                var result = _clientservice.GetById(id);
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
        public ActionResult Pay(Clients clients)
        {
            try
            {
                var c = 0;
                if (clients.Pay == 0)
                {
                    c++;
                }
                CollectionInfo collection = new CollectionInfo();
                collection.Amount = clients.Pay;
                collection.UserName = clients.ClientName;
                if (clients.ClientId == 0)
                {
                    clients.Due = clients.MonthlyBill + clients.ConnectionFee + clients.RouterFee + clients.Others - clients.Pay;


                }

                else
                {
                    if (clients.Due < clients.Pay)
                    {
                        ViewBag.msg = 2;
                        return RedirectToAction("Pay", "ClientInfo", new { id = clients.ClientId, id2 = 2 });
                    }
                    clients.Due = getdue(clients); 

                    collection.UserId = clients.ClientId;
                }
                clients.Pay = clients.Paid + clients.Pay;
                var result = _clientservice.Payment(clients);
                if (c == 0)
                    _collectionservice.Save(collection);
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
            return RedirectToAction("ClientShowAll", "ClientInfo");
        }

        public ActionResult FullPay(int id)
         {
             try
             {
                 var clients = _clientservice.GetById(id);
                 CollectionInfo collection = new CollectionInfo();
                 collection.Amount = clients.Data.Due;
                 collection.UserName = clients.Data.UserName;
                 var a = clients.Data.Due;
                clients.Data.Due = 0;
                collection.UserId = clients.Data.ClientId;

                clients.Data.Pay += a;
                var result = _clientservice.Payment(clients.Data);

                _collectionservice.Save(collection);
                if (clients.HasError)
                 {
                     ViewBag.Message = clients.Message;
                     return Content("Problem is : " + clients.Message);
                 }
             }
             catch (Exception ex)
             {

                 return Content(ex.Message);
             }
             return RedirectToAction("ClientShowAll", "ClientInfo");
         }

        [HttpPost]
        public ActionResult PayAll(FormCollection formCollection)
         {
             try
             {
                 string[] ids = formCollection["ClientId"].Split(new char[] {','});
                 foreach (string id in ids)
                 {
                     var i = Int32.Parse(id);
                     var clients = _clientservice.GetById(i);
                     CollectionInfo collection = new CollectionInfo();
                     collection.Amount = clients.Data.Due;
                     collection.UserName = clients.Data.UserName;
                     var a = clients.Data.Due;
                     clients.Data.Due = 0;
                     collection.UserId = clients.Data.ClientId;

                     clients.Data.Pay += a;
                     var result = _clientservice.Payment(clients.Data);

                     _collectionservice.Save(collection);
                     if (clients.HasError)
                     {
                         ViewBag.Message = clients.Message;
                         return Content("Problem is : " + clients.Message);
                     }
                 }
                
                
             }
             catch (Exception)
             {

                 return RedirectToAction("ClientShowAll", "ClientInfo",new{ id=1});
             }
             return RedirectToAction("ClientShowAll", "ClientInfo");
         }
       
    }
}
