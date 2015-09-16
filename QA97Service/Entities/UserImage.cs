using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
    public class UserImage
    {
        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }


        public virtual ApplicationUser User { get; set; }
    }
}
