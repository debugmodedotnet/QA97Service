using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
   public class School
    {

        public School()
        {
            Questions = new HashSet<Question>();
            Users = new HashSet<ApplicationUser>();

        }
       public int Id { get; set; }
       [Required]
       public string Name { get; set; }
       
       public string Email { get; set; }

       public string PhoneNumber { get; set; }

       public string AdminPassCode { get; set; }

       public string PassCode { get; set; }

       public string LogoUrl { get; set; }

       public string Address { get; set; }

        public int CityId { get; set;}

        public virtual City City { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
