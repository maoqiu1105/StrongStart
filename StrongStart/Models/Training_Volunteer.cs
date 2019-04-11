using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Training_Volunteer
    {
        public int ID { get; set; }
        
        public string volunteerID { get; set; }
        
        public int trainingID { get; set; }
        public Training training { get; set; }
    }
}
