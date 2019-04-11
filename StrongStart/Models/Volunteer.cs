using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Volunteer
    {
        public int volunteerID { get; set; }
        
        public string firstName { get; set; }
        
        public string lastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        public string PasswordSalt { get; set; }
        
        public string PasswordHash { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Phone { get; set; }
        
        public int infoID { get; set; }

        public int prefSchool { get; set; }
        public Site Site { get; set; }



    }
}
