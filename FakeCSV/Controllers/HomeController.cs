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

        public IActionResult EditScheme()
        {
            throw new NotImplementedException();
        }


        #region Edit Schema / Create New Schema

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                ViewBag.Title = "New Schema";
                return View(new NewSchemaViewModel { AddColumnOrder = 1 });
            }

            var schema = dataService.GetSchemaById((int)id);
            var model = new NewSchemaViewModel
            {
                Name = schema.Name,
                Separator = schema.Separator,
                Quotation = schema.Quotation,
                Columns = schema.Columns.Select(c => new ColumnViewModel
                {
                    ColumnName = c.Name,
                    LowerLimit = c.LowerLimit,
                    UpperLimit = c.UpperLimit,
                    Order = c.Order,
                    Type = c.Type,
                }).ToList(),
                AddColumnOrder = schema.Columns.Max(c => c.Order) + 1,

            };

            ViewBag.Title = $"Edit Schema {model.Name}";
            return View(model);
        }

        [HttpPost]
        public IActionResult AddColumn(NewSchemaViewModel model)
        {

            model.Columns ??= new List<ColumnViewModel>();

            if (model.AddColumnType == ColumnType.Text || model.AddColumnType == ColumnType.Integer)
            {
                model.Columns.Add(new()
                {
                    ColumnName = model.AddColumnName,
                    Type = model.AddColumnType,
                    Order = model.AddColumnOrder,
                    LowerLimit = model.AddColumnLowerLimit,
                    UpperLimit = model.AddColumnUpperLimit,
                });
            }
            else
            {
                model.Columns.Add(new()
                {
                    ColumnName = model.AddColumnName,
                    Type = model.AddColumnType,
                    Order = model.AddColumnOrder,
                });
            }



            model.AddColumnUpperLimit = 0;
            model.AddColumnName = null;
            model.AddColumnOrder = model.Columns.Max(c => c.Order) == 0
                ? 1
                : model.Columns.Max(c => c.Order) + 1;
            model.AddColumnType = 0;
            model.AddColumnLowerLimit = 0;

            ModelState.Clear();
            return View("Edit", model);
        }


        [HttpPost]
        public IActionResult Submit(NewSchemaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            if (model.Columns is null || !model.Columns.Any())
            {
                ModelState.AddModelError("", "Schema must contain at least one column");
                return View("Edit", model);
            }

            Schema schema = new Schema
            {
                Name = model.Name,
                Quotation = model.Quotation,
                Separator = model.Separator,
                CreationTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Columns = model.Columns.Select(c => new Column
                {
                    Name = c.ColumnName,
                    Type = c.Type,
                    Order = c.Order,
                    LowerLimit = c.LowerLimit,
                    UpperLimit = c.UpperLimit,
                })
                    .ToList()
            };

            var id = Request.RouteValues["id"];
            if (id != null)
            {
                schema.Id = int.Parse(id.ToString());
                dataService.UpdateSchema(schema);
                return RedirectToAction("Index", "Home");
            }

            dataService.AddSchema(schema);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DeleteColumn(NewSchemaViewModel model, int deleteid)
        {
            model.Columns.RemoveAt(deleteid);

            model.AddColumnUpperLimit = 0;
            model.AddColumnName = null;
            model.AddColumnOrder = model.Columns.Max(c => c.Order) == 0
                    ? 1
                    : model.Columns.Max(c => c.Order) + 1;
            model.AddColumnType = 0;
            model.AddColumnLowerLimit = 0;

            ModelState.Clear();
            return View("Edit", model);
        }


        #endregion



        public IActionResult DownloadCsv(string name)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult GenerateCsv(DataSetsPageViewModel model)
        {
            var schemaId = model.SchemaId;
            var rowsNumber = model.RowsNumber;

            dataService.GenerateData(schemaId, rowsNumber);

            return RedirectToAction("Data", new { id =model.SchemaId });
        }

        [HttpGet]
        public IActionResult Data(int id)
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
                return View(model);

            model.DataSets = dataSets.Select(d => new DataSetViewModel
            {
                Id = d.Id,
                CreationTime = d.CreationTime,
                Name = d.Name,
            });

            return View(model);
        }


    }
}
