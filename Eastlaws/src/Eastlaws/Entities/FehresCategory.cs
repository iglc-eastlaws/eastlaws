using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class FehresCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? MyOrder { get; set; }
        public int ProgCountryID { get; set; } 
    }
}
