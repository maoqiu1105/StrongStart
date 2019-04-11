using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Training_Trainer
    {
        public int ID { get; set; }

        
        public int trainingID { get; set; }
        public Training training { get; set; }

        [Display(Name = "Trainer")]
        public int trainerID { get; set; }
        public Trainer trainer { get; set; }

        [Display(Name = "Is this person a trainee?")]
        public YesNO becomeTrainer { get; set; }

        [Display(Name = "Trainee Status")]
        public Status traineeStatus { get; set; }

        [Display(Name = "Is this person bringing the kit?")]
        public YesNO hasKit { get; set; }

        [Display(Name = "Driving Distance")]
        public string driveDistance { get; set; }
    }

    public enum Status{
       NA,observing,leading
}
}
