using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
   public class AnswerVote
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Required]
        [StringLength(100)]
        public string ModifiedBy { get; set; }

        public int Score { get; set; }

        public int AnswerId { get; set; }

        public string UserId { get; set; }

        public virtual Answer Answer { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
