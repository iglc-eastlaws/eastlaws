using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class Images
    {
        public int ServiceTypeID { get; set; }
        public int ImageID { get; set; }
        public int ServiceID { get; set; }
        public string ImagePath { get; set; }
    }
}
