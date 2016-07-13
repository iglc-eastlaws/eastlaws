using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class AhkamRelatedDetails
    {
        //public IEnumerable<FehresCategory> CategoriesList { get; set; }
        public IEnumerable<FehresItem> FaharesList { get; set; }
        public IEnumerable<FehresLink> FaharesLinks { get; set; }
        public IEnumerable<FehresProgram> ProgramsList { get; set; }

    }
}
