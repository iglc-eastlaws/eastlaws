using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Eastlaws.ViewModels.Ahkam;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Eastlaws.Controllers
{
    public class AhkamController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GeneralSearch(SimpleInputSearch Model , int? Test)
        {
            return View(Model);
        }

        [HttpPost]
        public IActionResult CustomSearch(CustomSearchVM Model)
        {

            return View(Model);
        }


        public IActionResult Get(int? PageNo )
        {
            if (!PageNo.HasValue)
                PageNo = 1;

            string Query = "SELECT * FROM dbo.AH_Ma7akem am ";
            DataTable Table =  GetTable(Query);
            return View(Table);

            

        }
        private DataTable GetTable(string Query)
        {
            SqlConnection connection = new SqlConnection("Initial Catalog=iglc;Data Source=192.168.1.250;User ID=testDev;Password=test@myDev123;Asynchronous Processing=true;");
            SqlDataAdapter adapter = new SqlDataAdapter(Query, connection);
            DataTable dtData = new DataTable();
            adapter.Fill(dtData);
            return dtData;


        }

    }
}
