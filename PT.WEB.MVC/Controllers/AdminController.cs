using Microsoft.AspNet.Identity;
using PT.BLL.AccountRepository;
using PT.Entities.IdentitiyModel;
using PT.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PT.WEB.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            var userManager = MemberShipTools.NewUserManager();
            var users = userManager.Users.ToList().Select(x => new UsersViewModel
            {
                userId = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email,
                Username = x.UserName,
                RegisterDate = x.RegiterDate,
                Salary = x.Salary,
                RoleId = x.Roles.FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(y => y.Id == userManager.FindById(x.Id).Roles.FirstOrDefault().RoleId).Name
            }).ToList();
            return View(users);
        }

        public ActionResult EditUser(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            List<SelectListItem> rolList = new List<SelectListItem>();
            roles.ForEach(x => rolList.Add(new SelectListItem
            {
                Text = x.Name.ToString(),
                Value = x.Id.ToString()
            }));

            ViewBag.roles = rolList;

            var userManager = MemberShipTools.NewUserManager();
            var user = userManager.FindById(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var model = new UsersViewModel()
            {
                userId = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Username = user.UserName,
                Email = user.Email,
                Salary = user.Salary,
                RegisterDate = user.RegiterDate,
                RoleId = user.Roles.ToList().FirstOrDefault().RoleId,
                RoleName = roles.FirstOrDefault(y => y.Id == userManager.FindById(user.Id).Roles.FirstOrDefault().RoleId).Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> EditUser(UsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var roles = MemberShipTools.NewRoleManager().Roles.ToList();
            var userStore =  MemberShipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindById(model.userId);
            if (user==null)
            {
                return View("Index");
            }
            user.UserName = model.Username;
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.Salary = model.Salary;

            if (model.RoleId!=user.Roles.ToList().First().RoleId)
            {
                var yeniRoleName = roles.First(x => x.Id == model.RoleId).Name;
                userManager.AddToRole(model.userId,yeniRoleName);
                var eskiRoleName = roles.First(x => x.Id == user.Roles.ToList().First().RoleId).Name;
                userManager.RemoveFromRole(model.userId, eskiRoleName);
            }
            //await userManager.DeleteAsync(user);//--kullanici silme
            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("EditUser", new { id=model.userId});
        }

    }
}