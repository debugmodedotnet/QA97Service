using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.ViewModel
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DetailRichText { get; set; }
        public string DetailPlainText { get; set; }
        public int NumberOfAnswers { get; set; }
        public int Score { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
    }
}
