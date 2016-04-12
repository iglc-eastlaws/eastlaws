using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Infrastructure;

namespace Eastlaws.Entities
{
    public class AhkamPresentation
    {
        public int ResultsCount { get; set; }
        public IEnumerable<VW_Ahkam> AhkamList { get; set; }
        public IEnumerable<VW_AhkamFakarat> FakaratList { get; set; }
        public string QueryHash { get; private set; }
        private QueryInfo QueryInfo { get; set; }

        public AhkamPresentation()
        {
                
        }
    }
}
