using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
   public class Question
    {

        public Question()
        {
            Answers = new HashSet<Answer>();
            QuestionVotes = new HashSet<QuestionVote>();
            QuestionsImages = new HashSet<QuestionImage>();
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
        [StringLength(300)]
        public string QuestionTitle { get; set; }

        [Required]
        public string QuestionDetailPlainText { get; set; }

        [Required]

        public string QuestionDetailRichText { get; set; }

        public int ClassId { get; set; }

        public int SubjectId { get; set; }

        public string UserId { get; set; }

        public int? SchoolId { get; set; }

        public bool? isAnswered { get; set; }

        public bool? isPrivate { get; set; }



        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<QuestionImage> QuestionsImages { get; set; }

        public virtual Class Class { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual School School { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<QuestionVote> QuestionVotes { get; set; }


    }
}
