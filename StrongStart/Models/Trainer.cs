using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StrongStart.Class;


namespace StrongStart.Models
{
    public class Trainer
    {
        [Display(Name="Trainer")]
        public int trainerID { get; set; }
        public Title Title { get; set; }
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        public string lastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }

        public int regionID { get; set; }
        public Region region { get; set; }

        public ICollection<Training_Trainer> Training_Trainers { get; set; }
        
    }

    
    public enum Title
    {
        Trainer, Trainee
    }

}
