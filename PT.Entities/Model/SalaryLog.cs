using PT.Entities.IdentitiyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.Model
{
    [Table("SalaryLogs")]
    public class SalaryLog:BaseModel //ÖdemeLog
    {
        public decimal Salary { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
