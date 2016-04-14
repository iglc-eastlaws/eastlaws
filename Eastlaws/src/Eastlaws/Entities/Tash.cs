using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Eastlaws.Entities
{
    // write here because >>>>>>>>>>>>>  just for test
    public class tasfia {
        public int category { get; set; }
        public string categoryName { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        static public List<tasfia> GetTasfia()
        {
            /*
             ---------------- can be better
             catgoryname       return with seprate list object 
                               this object is contain  <catgID , catgName>  
             to prevent redundancy of catgName 

            */
            List<tasfia> ts= new List<tasfia>()
            {
                new tasfia{ category =1,categoryName = "catg Name 1",ID = 1,Name ="catg 1 name 1",Count=1},
                new tasfia{ category =1,categoryName = "catg Name 1",ID = 2,Name ="catg 1 name 2",Count=15},
                new tasfia{ category =1,categoryName = "catg Name 1",ID = 3,Name ="catg 1 name 3",Count=25},
                new tasfia{ category =1,categoryName = "catg Name 1",ID = 4,Name ="catg 1 name 4",Count=7},
                new tasfia{ category =1,categoryName = "catg Name 1",ID = 5,Name ="catg 1 name 5",Count=6},
                new tasfia{ category =2,categoryName = "catg Name 1",ID = 1,Name ="catg 2 name 1",Count=45},
                new tasfia{ category =2,categoryName = "catg Name 1",ID = 2,Name ="catg 2 name 2",Count=300},
                new tasfia{ category =2,categoryName = "catg Name 1",ID = 3,Name ="catg 2 name 3",Count=40},
                new tasfia{ category =2,categoryName = "catg Name 1",ID = 4,Name ="catg 2 name 4",Count=15},
                new tasfia{ category =2,categoryName = "catg Name 1",ID = 5,Name ="catg 2 name 5",Count=18},
                new tasfia{ category =3,categoryName = "catg Name 1",ID = 1,Name ="catg 3 name 1",Count=23},
                new tasfia{ category =3,categoryName = "catg Name 1",ID = 2,Name ="catg 3 name 2",Count=50},
                new tasfia{ category =3,categoryName = "catg Name 1",ID = 3,Name ="catg 3 name 3",Count=1},
                new tasfia{ category =3,categoryName = "catg Name 1",ID = 4,Name ="catg 3 name 4",Count=155},
                new tasfia{ category =3,categoryName = "catg Name 1",ID = 5,Name ="catg 3 name 5",Count=7},
            };

            return ts;
        }
    }

    public class Tash
    {
        public int ID { get; set; }
        public int Tash_No { get; set; }
        public int Tash_Year { get; set; }
        public string Text { get; set; }


        private SqlConnection _db = new SqlConnection("Initial Catalog=iglc;Data Source=192.168.1.250;User ID=testDev;Password=test@myDev123;Asynchronous Processing=true;");

        public List<Tash> GetAll()
        {
            return _db.Query<Tash>("SELECT TOP 100 [ID],[Tash_No] ,[Tash_Year],[Text] FROM [Tash_Master]").ToList();
        }

        public Tash Find(int ID)
        {
            return _db.Query<Tash>("SELECT  [ID],[Tash_No] ,[Tash_Year],[Text] FROM [Tash_Master] where ID = @ID", new { ID }).SingleOrDefault();
        }

    }
}
