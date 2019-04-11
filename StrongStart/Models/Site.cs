using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Site
    {
        public int siteID { get; set; }
        public string siteName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }

        public double geoLat { get; set; }
        public double geoLng { get; set; }

        public int regionID { get; set; }
        public Region region { get; set; }

        public ICollection<Training> Trainings { get; set; }
        public ICollection<Volunteer> Volunteers { get; set; }
    }
}
