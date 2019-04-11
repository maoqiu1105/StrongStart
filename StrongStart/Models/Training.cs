using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Training
    {
        public int trainingID { get; set; }

        [Display(Name = "Site Name")]
        public int siteID { get; set; }
        [Display(Name = "Site Name")]
        public Site site { get; set; }

        [Display(Name = "Term")]
        public int termID { get; set; }
        [Display(Name = "Term")]
        public Term term { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime startTime { get; set; }

        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime endTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0: dd MMM yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Do you have a permit?")]
        public YesNO permit { get; set; }

        [Display(Name = "Special Instructions For Volunteers")]
        public string specInstructions { get; set; }

        [Display(Name = "Training part (1 or 2)")]
        public Part part { get; set; }

        [Required]
        [Display(Name = "Capacity Limit")]
        public int Capacity { get; set; }

        public Training_Status training_Status { get; set; }

        [Display(Name = "Traning progress status")]
        public Training_Progress_Status training_Progress_Status { get; set; }

        [Display(Name = "Matching Part 1")]
        public int? linkID { get; set; }

        [Display(Name = "Training Name")]
        public string trainingName { get; set; }

        public ICollection<Training_Volunteer> Training_Volunteers { get; set; }

        public ICollection<Training_Trainer> Training_Trainers { get; set; }
    }
    public enum Part
    {
        part1, part2
    }
    public enum YesNO
    {
        No,Yes
    }
    public enum Training_Status
    {
        Active, Canceled
    }

    public enum Training_Progress_Status
    {
        Created,Approved,Published,Finished
    }
}
