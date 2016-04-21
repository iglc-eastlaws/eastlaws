using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Services
{
    public enum AhkamTasfeyaTypes
    {
        Country = 0, Ma7kama = 1, CrimeType = 2, RelatedTash = 3, Fehres = 4, Defoo3 = 5, Year = 6, JudgeYear = 7, Ma7kamaAction = 8
    }
    public class TasfeyaItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public AhkamTasfeyaTypes Category { get; set; }
    }

    public class AhkamTasfeya
    {
        public static string GetSectionTitle(AhkamTasfeyaTypes theType)
        {
            string Title = "";
            switch (theType)
            {
                case AhkamTasfeyaTypes.Country:
                    {
                        Title = "بالدولة"; break;
                    }
                case AhkamTasfeyaTypes.Ma7kama:
                    {
                        Title = "بالمحكمة"; break;
                    }
                case AhkamTasfeyaTypes.CrimeType:
                    {
                        Title = "بنوع الدعوى/ الجريمة"; break;
                    }
                case AhkamTasfeyaTypes.RelatedTash:
                    {
                        Title = "بالقانون"; break;
                    }
                case AhkamTasfeyaTypes.Fehres:
                    {
                        Title = "بتصنيف المبادئ"; break;
                    }
                case AhkamTasfeyaTypes.Defoo3:
                    {
                        Title = "بالدفوع"; break;
                    }
                case AhkamTasfeyaTypes.Year:
                    {
                        Title = "بسنة الحكم"; break;
                    }
                case AhkamTasfeyaTypes.JudgeYear:
                    {
                        Title = "بالسنة القضائية"; break;
                    }
                case AhkamTasfeyaTypes.Ma7kamaAction:
                    {
                        Title = "بحكم المحكمة فى الدعوى"; break;
                    }
            }
            return Title;
        }


    }
}
