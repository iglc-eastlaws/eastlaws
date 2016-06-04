using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class Country
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string FlagPic { get; set; }
    }


    public class FehresCountry
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string FlagPic { get; set; }
        public int ProgCountryID  { get; set; }
    }
}
