using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FMS.Core.Service.Interfaces;
using Newtonsoft.Json;
using ONS.App.FrameWork;
using ONS.Core.Entities;
using ONS.Core.Service.Interfaces;

namespace ONS.App.MVC.Controllers
{
    public class LogInController : Controller
    {
        private IAuthenticationService _service;
        private IUsersService _userservice;

        public LogInController(IAuthenticationService service, IUsersService userservice)
        {
            _service = service;
            _userservice = userservice;
        }

        public ActionResult LoginForm()
        {
            return View();

        }

        [HttpPost]
        public ActionResult LoginForm(Users user)
        {
            try
            {
                var result = _service.Login(user.UserName,user.Password);
                var obj = _service.GetByUserName(user.UserName.ToUpper());
                if (result.HasError)
                {
                    ViewBag.Message = result.Message;
                    return Content("Problem is : " + result.Message);
                }
                var jasonUserInfo = JsonConvert.SerializeObject(obj.Data);
                FormsAuthentication.SetAuthCookie(jasonUserInfo, false);
                if (result.Data.UserType.Contains("CLIENT"))
                    return RedirectToAction("Details", "ClientView");

                else
                    return RedirectToAction("ClientShowAll", "ClientInfo");
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
           
        }

        //public ActionResult RegistrationForm()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult RegistrationForm(Users user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("RegistrationForm", user);
        //    }
        //    try
        //    {

        //        var result = _userservice.Save(user);
        //        if (result.HasError)
        //        {
        //            ViewBag.Message = result.Message;
        //            return Content("Problem is : " + result.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return Content(ex.Message);
        //    }
        //    if (user.UserType.Contains("Client"))
        //        return RedirectToAction("ClientForm", "ClientInfo");

        //    else
        //        return RedirectToAction("Index", "Home");
        //}

    }
}
