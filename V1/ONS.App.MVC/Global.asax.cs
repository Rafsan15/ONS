using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ATP2.FMS;
using FMS.Core.Service.Interfaces;
using ONS.Core.Service.Interfaces;
using ONS.Service;
using Unity;

namespace ONS.App.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IUnityContainer container = new UnityContainer();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<ICollectionInfoService, CollectionInfoService>();
            container.RegisterType<ICostLogService, CostLogService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}
