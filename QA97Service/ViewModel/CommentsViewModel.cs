using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.ViewModel
{
    public class CommentsViewModel
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string CommentText { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
