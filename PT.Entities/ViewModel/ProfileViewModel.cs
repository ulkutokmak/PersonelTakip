using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.ViewModel
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name="Ad")]
        [StringLength(25)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Soyad")]
        [StringLength(35)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(25)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100,MinimumLength =5,ErrorMessage ="Şifeniz en az 5 karakter olmalıdır!")]
        [Display(Name = "Eski Şifre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifeniz en az 5 karakter olmalıdır!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifeniz en az 5 karakter olmalıdır!")]
        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Şifereler Uyuşmuyor!")]
        public string NewPasswordConfirm { get; set; }
    }
}
