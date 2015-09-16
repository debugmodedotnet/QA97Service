using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QA97Service.Entities
{
    public class City
    {
        public City()
        {
            Schools = new HashSet<School>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public virtual ICollection<School> Schools { get; set;}
    }
}