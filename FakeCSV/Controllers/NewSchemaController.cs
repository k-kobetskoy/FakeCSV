using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FakeCSV.Controllers
{
    [Authorize]
    public class NewSchemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
