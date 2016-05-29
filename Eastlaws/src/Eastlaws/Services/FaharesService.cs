﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Dapper;
using System.Text;

namespace Eastlaws.Services
{
    

    public class FaharesService
    {
      

        public IEnumerable<FehresProgram> GetProgramsByService (int? ServiceID)
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
            }
            else
            {
                // All Fehres Programs 
                WhereCondition = " Where (IfShow = 1 ) ";
            }
            string Query = " SELECT fp.ID, Name, AlterName, IfShow, MyOrder FROM dbo.FehresPrograms fp "
                + "\n" +  WhereCondition 
                + "\n" + " ORDER BY MyOrder ";


            using (var Connection = DataHelpers.GetConnection(DbConnections.Data))
            {
                return Connection.Query<FehresProgram>(Query);
            }              
            
        }


        public static IEnumerable<Country> GetCountriesByFehressID(int FehressID)
        {
            StringBuilder Q = new StringBuilder();
            Q.AppendFormat(@"select t2.ID,t2.Name,t2.EnName,t2.FlagPic
                                    from FehresProgCountry as t1
                                    inner join Countries as t2 on t1.CountryID = t2.ID
                                    where t1.FehresPogramID = {0}", FehressID);

            using (var Connection = DataHelpers.GetConnection(DbConnections.Data))
            {
                return Connection.Query<Country>(Q.ToString());
            }

        }



    }
}
