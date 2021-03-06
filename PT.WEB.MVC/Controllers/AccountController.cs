﻿using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
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
            checkUser = userManager.FindByEmail(model.Email);
            if (checkUser != null)
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresi kullanılmaktadır.");
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
                string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                //scheme:Http/https SchemeDelimiter:// Url.Host www vs. IsDefaultPort:localhosttaki gibi port numarası varsa yaz yoksa boş bırak.


                if (userManager.Users.Count() == 1)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    await SiteSettings.SendMail(new MailViewModel
                    {
                        To = user.Email,
                        Subject = "Hoşgeldin Sahip",
                        Message = "Sitemizi yöneteceğin için çok mutluyuz. ^^"
                    }); //await kullanırsak methodun başına async yazılır ve method void olmalı  void değilse Task <geridönüştipi> yazılır.
                }
                else
                {
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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Güvenlik Testleri için Gerekli--Arkaplanda Session oluşturur ve kendi formumuzdaki sessions la eşleşiyormu diye kontrol eder.
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userManager = MemberShipTools.NewUserManager();
            var user = await userManager.FindAsync(model.Username, model.Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Böyle bir kullanıcı bulunamadı.");
                return View(model);
            }
            var authManager = HttpContext.GetOwinContext().Authentication;
            var userIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            authManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = model.RememberMe//eğer işaretliyse true dönecek.
            }, userIdentity);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]//Sisteme Login olmadan Logout'u görememesi için
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Activation(string code)
        {
            var userStore = MemberShipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.ActivationCode == code);
            if (sonuc == null)
            {
                ViewBag.sonuc = "Aktivasyon işlemi Başarısız";
                return View();
            }
            sonuc.EmailConfirmed = true;
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();
             userManager.RemoveFromRole(sonuc.Id, "Passive");
             userManager.AddToRole(sonuc.Id, "User");
            ViewBag.sonuc = $"Merhaba {sonuc.Name}{sonuc.Surname}<br/> Aktivasyon işleminiz başarılı.";
            await SiteSettings.SendMail(new MailViewModel
            {
                To = sonuc.Email,
                Message = ViewBag.sonuc.ToString(),
                Subject = "Aktivasyon",
                Bcc = "poyildirim@gmail.com"
            });
            return View();
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> RecoverPassword(string email)
        {
            var userStore = MemberShipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var sonuc = userStore.Context.Set<ApplicationUser>().FirstOrDefault(x => x.Email == email);
            if (sonuc == null)
            {
                ViewBag.sonuc = "E-mail adresiniz sisteme kayıtlı değil.";
                return View();
            }
            var randomPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            await userStore.SetPasswordHashAsync(sonuc, userManager.PasswordHasher.HashPassword(randomPass));
            await userStore.UpdateAsync(sonuc);
            await userStore.Context.SaveChangesAsync();
            await SiteSettings.SendMail(new MailViewModel
            {
                To = sonuc.Email,
                Subject = "Şifre Değişikliği ",
                Message = $"Merhaba{sonuc.Name}{sonuc.Surname}<br/> Yeni Şifreniz: <b>{randomPass}</b> olarak değiştirilmiştir."
                    
            });
            ViewBag.sonuc = "Email adresinize yeni şifre gönderilmiştir.";
            return View();
        }

        [Authorize]
        public ActionResult Profile()
        {
            var userManager = MemberShipTools.NewUserManager();
            var user = userManager.FindById(HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId());
            var model = new ProfilePasswordViewModel()
            {
                ProfileViewModel= new ProfileViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Surname = user.Surname,
                    Name = user.Name,
                    Username = user.UserName
                }
               
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Profile(ProfilePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var userStore = MemberShipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindById(model.ProfileViewModel.Id);
                user.Name = model.ProfileViewModel.Name;
                user.Surname = model.ProfileViewModel.Surname;
                if (user.Email != model.ProfileViewModel.Email)
                {
                    user.Email = model.ProfileViewModel.Email;//aktivasyon gerekli
                    if (HttpContext.User.IsInRole("Admin"))
                    {
                        userManager.RemoveFromRole(user.Id, "Admin");
                    }
                    else if (HttpContext.User.IsInRole("User"))
                    {
                        userManager.RemoveFromRole(user.Id, "User");
                    }
                    userManager.AddToRole(user.Id, "Passive");
                    user.ActivationCode = Guid.NewGuid().ToString().Replace("-", "");
                    string siteUrl = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                    await SiteSettings.SendMail(new MailViewModel
                    {
                        To = user.Email,
                        Subject = "Personel Yönetimi - Aktivasyon",
                        Message = $"Merhaba{user.Name} {user.Surname}<br/>Email adresinizi<b>değiştirdiğiniz.</b> için hesabınızı tekrar aktif etmelisiniz.<a href='{siteUrl}/Account/Activation?code={user.ActivationCode}'>Aktivasyon İçin Tıklayınız.</a>"
                    });

                    HttpContext.GetOwinContext().Authentication.SignOut();
                    
                }
                await userStore.UpdateAsync(user);
                await userStore.Context.SaveChangesAsync();
                var model1 = new ProfilePasswordViewModel()
                {
                    ProfileViewModel = new ProfileViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Surname = user.Surname,
                        Name = user.Name,
                        Username = user.UserName
                    }

                };
                ViewBag.sonuc = "<b>Bilgileriniz Güncellenmiştir.</b>";
                return View(model1);
            }
            catch (Exception ex)
            {
                ViewBag.sonuc = ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePassword(ProfilePasswordViewModel model)
        {

            if (model.PasswordViewModel.NewPassword!=model.PasswordViewModel.NewPasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Şifreler uyuşmuyor");
                return View("Profile",model);
            }
            try
            {
                var userStore = MemberShipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindById(model.ProfileViewModel.Id);
                user = userManager.Find(user.UserName, model.PasswordViewModel.OldPassword);
                if (user==null)
                {
                    ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış girilmemiştir.");
                    return View("Profile", model);
                }
                await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(model.PasswordViewModel.NewPassword));
                await userStore.UpdateAsync(user);
                await userStore.Context.SaveChangesAsync();
                HttpContext.GetOwinContext().Authentication.SignOut();
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ViewBag.sonuc = "Güncelleme işleminde beklenmedik bir hata oluştu." + ex.Message;
                return View("Profile", model);
            }
           
        }

    }
}