using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.ViewModel
{
    public class AnswerViewModel
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public string AnswerDetailPlainText { get; set; }
        public string AnswerDetailRichText { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
        
    }
}
