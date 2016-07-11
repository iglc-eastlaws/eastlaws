using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Entities
{
    public class FehresItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
        public int FehresCategoryID { get; set; }
        public int? MyOrder { get; set; }
        public int Level { get; set; }
        public string Path { get; set; }
        public string FullName { get; set; }
        public bool IsLastLevel { get; set; }
    }

    public class FehresLink
    {
        public int FehresItemID { get; set; }
        public int MasterItemID { get; set; }
        public int ChildItemID { get; set; }
        public int? StartColor { get; set; }
        public int? EndColor { get; set; }
    }
}
