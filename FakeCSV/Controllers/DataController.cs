using FakeCSV.Domain.ViewModels;
using FakeCSV.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace FakeCSV.Controllers
{
    [Authorize]
    public class DataController : Controller
    {
        private readonly ISchemaDataService dataService;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly ILogger<DataController> logger;

        public DataController(
            ISchemaDataService dataService,
            IWebHostEnvironment appEnvironment,
            ILogger<DataController> logger)
        {
            this.dataService = dataService;
            this.appEnvironment = appEnvironment;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int id, bool? generationError = false)
        {
            if (generationError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Generation CSV Failed. Try again.";

            var schema = dataService.GetSchemaById(id);
            ViewBag.Title = $"Data Sets for {schema.Name}";
            var model = new DataSetsPageViewModel
            {
                SchemaId = id,
                SchemaName = schema.Name,
                RowsNumber = 1,
            };

            var dataSets = schema.DataSets.ToList();
            if (!dataSets.Any())
            {
                model.DataSetQuantity = 0;
                return View(model);
            }

            model.DataSets = dataSets.Select(d => new DataSetViewModel
            {
                Id = d.Id,
                CreationTime = d.CreationTime,
                Name = d.Name,
            });
            model.DataSetQuantity = model.DataSets.Count();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateCsv(int id, int rows, [FromServices] IGenerateCsvService csvService)
        {
            var schemaId = id;
            var rowsNumber = rows;
            int dataSetId;

            try
            {
                dataSetId = await csvService.GenerateData(schemaId, rowsNumber);
            }
            catch (Exception e)
            {
                logger.LogError("Error while generating csv file {0}", e);
                return RedirectToAction("Index", new {id = id, generationError = true});
            }


            var dataSet = dataService.GetDatasetById(dataSetId);

            var result = dataSet.Name;
            return Content(result);
        }


        public PhysicalFileResult DownloadCsv(string name)
        {
            string path = appEnvironment.ContentRootPath + $@"\files\{name}";
            string type = "application/csv";
            string fileName = name;
            return PhysicalFile(path, type, fileName);
        }


        [HttpGet]
        public IActionResult AppendNewRow(int number)
        {
            var model = new PartialTableRowViewModel()
            {
                RowNumber = number,
                CreationDate = DateTime.Now,

            };
            return PartialView("Partial/_DataTableRow", model);
        }



    }
}
