using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models.ViewModel
{
    public class AccountDashboard
    {
        [Required]

        public int ID { get; set; }

        [Required]
        [DisplayName("First Name")]

        public string firstName { get; set; }

        [Required]
        [DisplayName("Last Name")]

        public string lastName { get; set; }


        [Required]

        public string Email { get; set; }

        [Required]

        public string Address { get; set; }

        [Required]

        public string Phone { get; set; }

        [Required]

        public string City { get; set; }

        [Required]

        public string Province { get; set; }

        [Required]

        public string volunteerId { get; set; }

        public List<Training> trainingSessions { get; set; }
    }
}
