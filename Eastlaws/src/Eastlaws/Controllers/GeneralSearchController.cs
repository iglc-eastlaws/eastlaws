using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eastlaws.Infrastructure;

namespace Eastlaws.Controllers
{
    public class GeneralSearchController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult test()
        {
            string inputtext = "القتل    العمد مع   سبق الاصرار و الترصد";
            string outputtextand = TextHelpers.Build_and(
                TextHelpers.RemoveNoiseWords(inputtext),Build_Option.and
                ).ToString();

            string outputtextor = TextHelpers.Build_and(inputtext, Build_Option.or).ToString();

            int x = 0;
            return View();
        }
    }
}
