using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ONS.Core.Entities;

namespace ONS.App.MVC.View_Model
{
    public class ClientFormModel
    {
        public List<Clients> Clients=new List<Clients>();
        public List<double> Due =new List<double>();
    }
}