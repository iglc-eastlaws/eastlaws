using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eastlaws.Services
{
    public enum AhkamTasfeyaCategories
    {
        Country = 0, Ma7kama = 1, CrimeType = 2, RelatedTash = 3, Fehres = 4, Defoo3 = 5, Year = 6, JudgeYear = 7, Ma7kamaAction = 8
    }
    public class TasfeyaItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public AhkamTasfeyaCategories Category { get; set; }
    }

    public class AhkamTasfeya
    {
        public const int MAX_RECORD_COUNT_PER_CATEGORY = 100;


        public static List<TasfeyaItem> List (int QueryID , AhkamSearchTypes SearchType , string TasfeyaFilter )
        {
            List<AhkamTasfeyaCategories> Cats = new List<AhkamTasfeyaCategories>();
            Cats.Add(AhkamTasfeyaCategories.Country);
            Cats.Add(AhkamTasfeyaCategories.Ma7kama);
            Cats.Add(AhkamTasfeyaCategories.JudgeYear);
            Cats.Add(AhkamTasfeyaCategories.Year);


            string MasterQuery = "";

            return null;
        }

        private static string GetCategoryQuery(int MasterQueryID , AhkamTasfeyaCategories Cat , string Filter, bool isSafyList = false)
        {
            switch (Cat )
            {
                case AhkamTasfeyaCategories.Country:
                    {
                        return null;
  
                    }
                default:
                    {
                        return null;
               
                    }
            }
        }

        public static string GetSectionTitle(AhkamTasfeyaCategories theType)
        {
            string Title = "";
            switch (theType)
            {
                case AhkamTasfeyaCategories.Country:
                    {
                        Title = "بالدولة"; break;
                    }
                case AhkamTasfeyaCategories.Ma7kama:
                    {
                        Title = "بالمحكمة"; break;
                    }
                case AhkamTasfeyaCategories.CrimeType:
                    {
                        Title = "بنوع الدعوى/ الجريمة"; break;
                    }
                case AhkamTasfeyaCategories.RelatedTash:
                    {
                        Title = "بالقانون"; break;
                    }
                case AhkamTasfeyaCategories.Fehres:
                    {
                        Title = "بتصنيف المبادئ"; break;
                    }
                case AhkamTasfeyaCategories.Defoo3:
                    {
                        Title = "بالدفوع"; break;
                    }
                case AhkamTasfeyaCategories.Year:
                    {
                        Title = "بسنة الحكم"; break;
                    }
                case AhkamTasfeyaCategories.JudgeYear:
                    {
                        Title = "بالسنة القضائية"; break;
                    }
                case AhkamTasfeyaCategories.Ma7kamaAction:
                    {
                        Title = "بحكم المحكمة فى الدعوى"; break;
                    }
            }
            return Title;
        }


    }
}
