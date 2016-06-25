using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Entities;
using Eastlaws.Infrastructure;
using Eastlaws;
using Dapper;
using System.Data.SqlClient;

namespace Eastlaws.Services
{
    public class ImageService
    {
        public static IEnumerable<Images> GetImages(int ServiceID)
        {
            SqlConnection con = DataHelpers.GetConnection(DbConnections.Data);
            var Data = con.Query<Images>("GetServiceImages", new { ServiceTypeID = 1 , ServiceID = ServiceID }, null, true, null, System.Data.CommandType.StoredProcedure);
            return Data;
        }

    }
}
