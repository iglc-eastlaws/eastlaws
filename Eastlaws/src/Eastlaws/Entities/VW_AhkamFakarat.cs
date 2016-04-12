using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class VW_AhkamFakarat
    {
        public int ID { get; set; }
        public int? HokmID { get; set; }
        public int FakraNo { get; set; }
        public string Text { get; set; }
        public string EnText { get; set; }
        public string MogazText { get; set; }
        public int? TashCount { get; set; }
        public int? TashMawadCount { get; set; }
        public string Title { get; set; }
        public int? MyOrder { get; set; }        }
}
