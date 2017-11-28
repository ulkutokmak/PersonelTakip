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
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//Güvenlik Testleri için Gerekli--Arkaplanda Session oluşturur ve kendi formumuzdaki sessions la eşleşiyormu diye kontrol eder.
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MemberShipTools.NewUserManager();
            var checkUser = userManager.FindByName(model.Username);
            if (checkUser!=null)
            {
                ModelState.AddModelError(string.Empty, "Bu Kullanıcı Zaten Kayıtlı !");
                return View(model);
            }
            return View();
        }
    }
}