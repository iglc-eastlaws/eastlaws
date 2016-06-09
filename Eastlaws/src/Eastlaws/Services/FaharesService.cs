using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Dapper;
using System.Text;
using System.Data;

namespace Eastlaws.Services
{
    

    public class FaharesService
    {
      

        public static IEnumerable<FehresProgram> GetProgramsByService (int? ServiceID)
        {
            string WhereCondition = "";
            if (ServiceID.HasValue)
            {
                
                if(ServiceID.Value == 1)// Ahkam 
                {
                    WhereCondition = " Where fp.ID  In ( 9,15 ,29,30) "; 
                }
                else if(ServiceID.Value == 2)// Tashree3at 
                {
                    WhereCondition = "";
                }
                else if(ServiceID.Value == 3)
                {
                    WhereCondition = "";
                }
            }
            else
            {
                // All Fehres Programs 
                WhereCondition = " Where (fp.IfShow = 1 ) ";
            }
            string Query = " SELECT fp.ID, fp.Name, fp.AlterName, fp.IfShow, fp.MyOrder FROM dbo.FehresPrograms fp "
                + "\n" +  WhereCondition 
                + "\n" + " ORDER BY fp.MyOrder ";


            using (var Connection = DataHelpers.GetConnection(DbConnections.Data))
            {
                return Connection.Query<FehresProgram>(Query);
            }              
            
        }


        public static IEnumerable<FehresCountry> GetCountriesByFehressID(int FehresProgramID)
        {
            StringBuilder Q = new StringBuilder();
            Q.AppendFormat(@"select t2.ID,t2.Name,t2.EnName,t2.FlagPic , t1.ID as ProgCountryID
                                    from FehresProgCountry as t1
                                    inner join Countries as t2 on t1.CountryID = t2.ID
                                    where t1.FehresPogramID = {0}", FehresProgramID);



            using (var Connection = DataHelpers.GetConnection(DbConnections.Data))
            {
                return Connection.Query<FehresCountry>(Q.ToString());
            }
        }

        public static IEnumerable<FehresCategory> GetCategoriesByProgCountry(int ProgCountry)
        {
            string Query = @"SELECT fc.ID , fc.Name , fc.MyOrder , fc.ProgCountryID FROM 
                            dbo.FehresProgCountry fpc	
                            JOIN dbo.FehresCategories fc ON fc.ProgCountryID = fpc.ID
                            Where fc.ProgCountryID = {0}
                            ORDER BY MyOrder";

            Query = string.Format(Query, ProgCountry);
            using (var Connection = DataHelpers.GetConnection(DbConnections.Data))
            {
                return Connection.Query<FehresCategory>(Query.ToString());
            }
        
        }


        public static IEnumerable<FehresItem> GetItems(int CategoryID , string SearchText = null)
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchText = new FTSPredicate(SearchText, FTSSqlModes.AND).BuildPredicate();
            }
            else
            {
                SearchText = null;
            }

            DynamicParameters Params = new DynamicParameters();
            Params.Add("@CategoryID", CategoryID);
            Params.Add("@SearchText", SearchText);
            CommandDefinition Cmd = new CommandDefinition(commandText: "GetFehresItems",parameters  : Params ,commandType : CommandType.StoredProcedure);
            using (var Connection = DataHelpers.GetConnection(DbConnections.Data))
            {
                return Connection.Query<FehresItem>(Cmd);
            }
        }

        



    }
}
