using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Region
    {
        public int regionID { get; set; }

        [Display(Name = "Region Code")]
        public string regionCode { get; set; }

        [Display(Name = "Region Name")]
        public string regionName { get; set; }
        
        public ICollection<Site> Sites { get; set; }
        public ICollection<Trainer> Trainers { get; set; }
    }
}
