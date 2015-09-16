using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
   public class Answer
   {
       public Answer()
       {
           AnswerVotes = new HashSet<AnswerVote>();
           AnswerImages = new HashSet<AnswerImage>();
       }
    
       public int Id { get; set; }

       public DateTime CreatedDate { get; set; }

       public DateTime ModifiedDate { get; set; }

       [Required]
       [StringLength(100)]
       public string CreatedBy { get; set; }

       [Required]
       [StringLength(100)]
       public string ModifiedBy { get; set; }

       [Required]
       public string AnswerDetailPlainText { get; set; }

       [Required]
       public string AnswerDetailRichText { get; set; }

       public int QuestionId { get; set; }

       public string UserId { get; set; }

       public bool? isAcceptedAnswer { get; set; }


       public virtual Question Question { get; set; }

       public virtual ApplicationUser User { get; set; }

       public virtual ICollection<AnswerImage> AnswerImages { get; set; }

       public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
    }
}
