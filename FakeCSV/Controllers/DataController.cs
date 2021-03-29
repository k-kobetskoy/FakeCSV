﻿using FakeCSV.Domain.ViewModels;
using FakeCSV.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FakeCSV.Controllers
{
    public class DataController : Controller
    {
        private readonly ISchemaDataService dataService;

        public DataController(ISchemaDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
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

            var dataSetId = await csvService.GenerateData(schemaId, rowsNumber);
            var dataSet = dataService.GetDatasetById(dataSetId);

            var result = dataSet.Name;
            return Content(result);
        }


        public IActionResult DownloadCsv(string name)
        {
            throw new NotImplementedException();
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
