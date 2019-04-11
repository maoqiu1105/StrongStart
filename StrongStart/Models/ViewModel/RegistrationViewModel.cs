using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models.ViewModel
{
    public class RegistrationViewModel
    {
        public string siteName { get; set; }

        public string Date { get; set; }

        public string startTime { get; set; }

        public string endTime { get; set; }

        public Part part { get; set; }

        public Training_Status training_Status { get; set; }

        public string Capacity { get; set; }
        
        public Training_Progress_Status Training_Progress_Status { get; set; }

        public int trainingID { get; set; }
    }
}
