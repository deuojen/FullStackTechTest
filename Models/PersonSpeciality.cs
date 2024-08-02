using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PersonSpeciality
    {
        public int Id { get; set; }
        public string PersonId { get; set; }
        public string SpecialityId { get; set; }
        public string SpecialityName { get; set; }
    }
}
