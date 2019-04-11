using Microsoft.AspNetCore.Identity;
using StrongStart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string Province { get; set; }
        [PersonalData]
        public string prefSchool { get; set; }

        public int FinishPart1 { get; set; }
        public int FinishPart2 { get; set; }
        public YesNO isQualify { get; set; }
    }
}