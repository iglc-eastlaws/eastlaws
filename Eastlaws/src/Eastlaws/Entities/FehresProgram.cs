using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class FehresProgram
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string AlterName { get; set; }
        public int? MyOrder { get; set; }
        public bool? IfShow { get; set; }
    }
}
