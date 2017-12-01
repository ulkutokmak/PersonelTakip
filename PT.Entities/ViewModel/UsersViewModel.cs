using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.Entities.ViewModel
{
    public class UsersViewModel
    {
        public string userId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public decimal Salary { get; set; }
        public DateTime RegisterDate { get; set; }

    }
}
