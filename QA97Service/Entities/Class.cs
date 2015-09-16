using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA97Service.Entities
{
  public class Class
    {
        public Class()
        {
            Questions = new HashSet<Question>();
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
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
