﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(PT.WEB.MVC.App_Start.Startup))]

namespace PT.WEB.MVC.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType="ApplicationCookie",
                LoginPath=new PathString("/Account/Login")//Authentication'ı geçemezse eğer direk login'e yönlendirir.
            });
    }
}
}
