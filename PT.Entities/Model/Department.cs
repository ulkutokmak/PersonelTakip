using PT.Entities.IdentitiyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.Model
{
    [Table("Departments")]
    public class Department:BaseModel
    {
        [Required] //null olabilir
        [StringLength(55,ErrorMessage="Bu alan zorunludur",MinimumLength=5)]
        [Index(IsUnique =true)] //Departman adı eşssiz olsun diye
        public string DepartmentName { get; set; }

        public virtual List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
