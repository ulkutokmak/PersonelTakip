using Microsoft.AspNet.Identity;
using PT.BLL.AccountRepository;
using PT.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PT.WEB.MVC.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            var userManager = MemberShipTools.NewUserManager();
            var users = userManager.Users.Select(x=> new UsersViewModel
            {
                userId=x.Id,
                Name=x.Name,
                Surname=x.Surname,
                Email=x.Email,
                Username=x.UserName,
                RegisterDate=x.RegiterDate,
                Salary=x.Salary,
                RoleId=x.Roles.FirstOrDefault().RoleId,
                RoleName=roles.FirstOrDefault(y=>y.Id==x.Roles.FirstOrDefault().RoleId).Name
            }).ToList();
            
            List<SelectListItem> rolList = new List<SelectListItem>();
            roles.ForEach(x => new SelectListItem
            {
                Text=x.Name,
                Value=x.Id
            });
            ViewBag.roles = rolList;
            return View(users);
        }
    }
}