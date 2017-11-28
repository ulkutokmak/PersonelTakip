using Microsoft.AspNet.Identity;
using PT.BLL.AccountRepository;
using PT.BLL.Settings;
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
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//Güvenlik Testleri için Gerekli--Arkaplanda Session oluşturur ve kendi formumuzdaki sessions la eşleşiyormu diye kontrol eder.
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //Kayıt olmadan önce kontrol edilir.
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MemberShipTools.NewUserManager();
            var checkUser = userManager.FindByName(model.Username);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu Kullanıcı Zaten Kayıtlı !");
                return View(model);
            }
            //Register İşlemi Yapılır.
            var activationCode = Guid.NewGuid().ToString();//Guid benzersiz bir id oluşturur.
            ApplicationUser user = new ApplicationUser()
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Username,
                ActivationCode = activationCode
            };
            var response = userManager.Create(user, model.Password);//Passwordu hash etme işlemini Create methodunda yapar.
            if (response.Succeeded)//kayıt başarılı ise
            {
                string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : "" + Request.Url.Port);  
                //scheme:Http/https SchemeDelimiter:// Url.Host www vs. IsDefaultPort:localhosttaki gibi port numarası varsa yaz yoksa boş bırak.


                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    await SiteSettings.SendMail(new MailViewModel
                    {
                        To=user.Email,
                        Subject="Hoşgeldin Sahip",
                        Message="Sitemizi yöneteceğin için çok mutluyuz. ^^"
                    }); //await kullanırsak methodun başına async yazılır ve method void olmalı  void değilse Task <geridönüştipi> yazılır.
                }
                else{
                    userManager.AddToRole(user.Id, "Passive");
                    await SiteSettings.SendMail(new MailViewModel
                    {
                        To = user.Email,
                        Subject = "Personel Yönetimi - Aktivasyon",

                        /* Message = $"Merhaba{user.Name} {user.Surname} <br/> Sistemi kullanabilmeniz için <a href='http://localhost:28442/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a>" */

                        Message = $"Merhaba{user.Name} {user.Surname} <br/> Sistemi kullanabilmeniz için <a href='{siteUrl}/Account/Activation?code={activationCode}'>Aktivasyon Kodu</a>" //$ doları koyduğumuzda girilecek c# kodlarını tanır.
                    });
                }
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt işleminde bir hata oluştu.");
                return View(model);
            }
        }
    }
}