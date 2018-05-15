using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ONS.Core.Entities;

namespace ONS.App.FrameWork
{
    public static class HttpUtil
    {
        public static Users CurrentUser
        {
            get
            {
                try
                {
                    var user = JsonConvert.DeserializeObject<Users>(HttpContext.Current.User.Identity.Name);
                    return user;
                }
                catch (Exception )
                {

                    return null;
                }
            }
        }
    }
}
