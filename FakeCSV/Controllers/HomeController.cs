using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.Domain.Models;
using FakeCSV.Domain.ViewModels;
using FakeCSV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace FakeCSV.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISchemaDataService dataService;

        public HomeController(ISchemaDataService dataService)
        {
            this.dataService = dataService;
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

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View();
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
                return View(new NewSchemaViewModel());
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
                }).ToList()
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
            model.AddColumnOrder = 0;
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
            model.AddColumnOrder = 0;
            model.AddColumnType = 0;
            model.AddColumnLowerLimit = 0;

            ModelState.Clear();
            return View("Edit", model);
        } 

        #endregion




    }
}
