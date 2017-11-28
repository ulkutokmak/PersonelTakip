using Microsoft.AspNet.Identity.EntityFramework;
using PT.Entities.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.IdentitiyModel
{
    public class ApplicationUser:IdentityUser
    {
        [StringLength(25)]
        public string Name { get; set; }

        [StringLength(35)]
        public string Surname { get; set; }
        
        public decimal Salary { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime RegiterDate { get; set; } = DateTime.Now;//kayıt olunan tarihi direk alır.AutoSetter.

        public int? DepertmentID { get; set; }

        [ForeignKey("DepertmentID")]
        public virtual Department Department { get; set; }

        public virtual List<LaborLog> LaborLogs { get; set; } = new List<LaborLog>();//constructor la uğraşmamak için. Null geçilmemesini sağlar.

        public virtual List<SalaryLog> SalaryLogs { get; set; } = new List<SalaryLog>();//constructor la uğraşmamak için. Null geçilmemesini sağlar.
    }
}
