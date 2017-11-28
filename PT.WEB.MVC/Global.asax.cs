using Microsoft.AspNet.Identity;
using PT.BLL.AccountRepository;
using PT.Entities.IdentitiyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PT.WEB.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var roleManager = MemberShipTools.NewRoleManager();
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Admin",
                    Description = "Sistem Yöneticisi"
                });
            }
            if (!roleManager.RoleExists("User"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "User",
                    Description = "Sistem Kullanıcısı"
                });
            }
            if (!roleManager.RoleExists("Passive"))
            {
                roleManager.Create(new ApplicationRole()
                {
                    Name = "Passive",
                    Description = "E-Mail Aktivasyonu Gerekli !"
                });
            }
        }
    }
}
