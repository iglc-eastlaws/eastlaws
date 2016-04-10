using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public enum cuisineType
    {
        None, Italian, French, American
    }
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public cuisineType cuisine { get; set; }
    }

}
