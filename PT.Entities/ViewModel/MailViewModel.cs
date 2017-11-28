using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.ViewModel
{
    public class MailViewModel
    {
        public string To { get; set; } //Kime Gidecek
        public List<String> ToList { get; set; } = new List<string>(); //Toplu Mesaj
        public string Subject { get; set; } //Konu
        public string Message { get; set; } //Mesaj
        public string Cc { get; set; } //Cc
        public string Bcc { get; set; } //Bcc-Gizli
    }
}
