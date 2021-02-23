using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeCSV.Controllers
{
    public class NewSchemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
