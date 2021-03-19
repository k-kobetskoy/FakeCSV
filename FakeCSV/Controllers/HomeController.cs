using FakeCSV.Domain.Models;
using FakeCSV.Domain.ViewModels;
using FakeCSV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace FakeCSV.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISchemaDataService dataService;
        private readonly ILogger<HomeController> logger;

        public HomeController(ISchemaDataService dataService, ILogger<HomeController> logger)
        {
            this.dataService = dataService;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var schemas = dataService.GetSchemas();
            if (schemas is null)
                return View(new List<SchemaViewModel>());

            var model = new List<SchemaViewModel>(
                schemas.OrderBy(s => s.UpdateTime)
                    .Select(schema => new SchemaViewModel
                    {
                        Id = schema.Id,
                        Name = schema.Name,
                        CreationTime = (DateTime)schema.CreationTime,
                        UpdateTime = (DateTime)schema.UpdateTime,
                    }));
            ViewBag.Title = "Data schemas";
            return View(model);
        }

        public IActionResult DeleteScheme()
        {
            throw new NotImplementedException();
        }
    }
}
