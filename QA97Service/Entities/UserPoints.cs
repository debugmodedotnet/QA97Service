using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
   public class UserPoint
    {
        [ForeignKey("User")]
        public string Id { get; set; }
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

        public int  Points { get; set; }

       
        

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
