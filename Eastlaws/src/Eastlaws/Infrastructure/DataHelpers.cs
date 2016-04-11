using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Eastlaws.Infrastructure
{
    public enum DbConnections
    {
        Data = 0 , Users = 1 , GeneralSearch = 2
    }
    public class DataHelpers
    {
        public static string[] ClientDateFormats =
        {
            "dd/MM/yyyy"
            ,"dd-MM-yyyy"
        };

        public static SqlConnection GetConnection(DbConnections data)
        {
            string DatabaseName = "";
            switch (data)
            {
                case DbConnections.Data:
                    DatabaseName = "EastlawsData";
                    break;
                case DbConnections.Users:
                    DatabaseName = "EastlawsUsers";
                    break;
                case DbConnections.GeneralSearch:
                    DatabaseName = "EastlawsGeneralSearch";
                    break;
            }
            string ConString = "Server=192.168.1.251;Database="+ DatabaseName + ";User ID=DevUser;Password=DevUser123456;";
            SqlConnection con = new SqlConnection(ConString);
            return con;
        }

    }


}
