using PT.Entities.IdentitiyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.Model
{
    [Table("LaborLogs")]
    public class LaborLog:BaseModel //ÇalışmaLog
    {
        public DateTime StartShift { get; set; } = DateTime.Now;

        public DateTime? EndShift { get; set; } //Giriş yapmadan çıkış yapamayız.

        public string UserId { get; set; } //string olmasının nedeni:ApllicationUserda IdentitiyUser'dan kalıtım aldığımız için ıd ordan atanıyor ve tipi string.

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
