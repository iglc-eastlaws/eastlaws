using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Eastlaws.Entities
{
    //only for test
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
