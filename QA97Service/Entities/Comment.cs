using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
    public class Comment
    {

        public int Id { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }

        [Required]
        public string CommentText { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual Question Question { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}
