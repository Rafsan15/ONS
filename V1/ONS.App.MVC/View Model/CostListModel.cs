using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ONS.Core.Entities;

namespace ONS.App.MVC.View_Model
{
    public class CostListModel
    {
        public List<CostLog>CostLogs=new List<CostLog>();
        public List<Employees> Employees = new List<Employees>(); 
    }
}