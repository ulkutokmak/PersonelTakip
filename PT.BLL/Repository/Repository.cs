using PT.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.BLL.Repository
{
    public class DepartmentRep : RepositoryBase<Department, int> {}
    public class SalaryLogRep : RepositoryBase<SalaryLog, int>{}
    public class LaborLogRep : RepositoryBase<LaborLog, int>{}
}
