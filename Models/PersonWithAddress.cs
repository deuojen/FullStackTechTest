using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PersonWithAddress : Person
    {
        public List<Address> Address { get; set; }
    }
}
