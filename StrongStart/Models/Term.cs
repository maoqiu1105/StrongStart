using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongStart.Models
{
    public class Term
    {
        public int termID { get; set; }
        public string termName { get; set; }

        public ICollection<Training> Trainings { get; set; }
    }
}
