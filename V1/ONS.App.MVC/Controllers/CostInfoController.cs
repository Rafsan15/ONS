using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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