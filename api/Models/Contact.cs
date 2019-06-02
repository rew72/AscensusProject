using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Contact
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int salary { get; set; }
        public int hourly { get; set; }
        public string role { get; set; }
    }
}
