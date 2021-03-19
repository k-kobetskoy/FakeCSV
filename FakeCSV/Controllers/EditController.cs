using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.Domain.Models;
using FakeCSV.Domain.ViewModels;
using FakeCSV.Services;

namespace FakeCSV.Controllers
{
    public class EditController : Controller
    {
        private readonly ISchemaDataService dataService;

        public EditController(ISchemaDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public IActionResult Index(int? id)
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
            return View("Index", model);
        }


        [HttpPost]
        public IActionResult Submit(NewSchemaViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            if (model.Columns is null || !model.Columns.Any())
            {
                ModelState.AddModelError("", "Schema must contain at least one column");
                return View("Index", model);
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
            var values = Request.RouteValues;
            var id = values["id"];
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
            return View("Index", model);
        }

    }
}
