using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Eastlaws.Infrastructure
{
    public enum DbConnections
    {
        Data = 0 , Users = 1 , GeneralSearch = 2
    }

    // temporary , All DB Connection strings should be obtained from the Configuration file !
    public class DataHelpers
    {
        public static SqlConnection GetConnection()
        {
            return GetConnection(DbConnections.Data);
        }
        public static SqlConnection GetConnection(DbConnections Connection)
        {
            string ConString = GetConnectionsList()[Connection];
            SqlConnection con = new SqlConnection(ConString);
            con.Open();
            return con;
        }
        private static Dictionary<DbConnections , string> GetConnectionsList()
        {
            Dictionary<DbConnections, string> ConnectionsList = new Dictionary<DbConnections, string>();
            ConnectionsList.Add(DbConnections.Data, "Server=192.168.1.251;Database=EastlawsData;User ID=DevUser;Password=DevUser123456");
            ConnectionsList.Add(DbConnections.Users, "Server=192.168.1.251;Database=EastlawsUsers;User ID=DevUser;Password=DevUser123456");
            ConnectionsList.Add(DbConnections.GeneralSearch, "Server=192.168.1.251;Database=EastlawsGeneralSearch;User ID=DevUser;Password=DevUser123456");
            return ConnectionsList;
        }

    }


    // recomanded to use non static class in booling 
    // https://msdn.microsoft.com/en-us/library/8xx3tyca(v=vs.110).aspx
    public enum DataBases
    {
        Data,Users, GeneralSearch
    }
    public class DataHelpers2
    {
        string database = "EastlawsData";
        public DataHelpers2(DataBases data) {
            switch (data)
            {
                case DataBases.Data:
                    database = "EastlawsData";
                    break;
                case DataBases.Users:
                    database = "EastlawsUsers";
                    break;
                case DataBases.GeneralSearch:
                    database = "EastlawsGeneralSearch";
                    break;

            }
        }
        public SqlConnection GetConnection()
        {
            string ConString = "Server=192.168.1.251;Database="+ database + ";User ID=DevUser;Password=DevUser123456";
            SqlConnection con = new SqlConnection(ConString);
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
            }
            return con;
        }
    }


}
