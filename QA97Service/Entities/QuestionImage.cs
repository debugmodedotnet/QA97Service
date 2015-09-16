using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA97Service.Entities
{
    public class QuestionImage
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

        public string  ImageUrl { get; set; }

        public int QuestionId { get; set; }


        public virtual Question Question { get; set; }

       
    }
}