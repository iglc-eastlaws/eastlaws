using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Dapper;


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





    }
}
