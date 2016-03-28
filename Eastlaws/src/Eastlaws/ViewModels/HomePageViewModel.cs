using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;

namespace Eastlaws.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public string CurrentGreeeting { get; set; }
    }
}
