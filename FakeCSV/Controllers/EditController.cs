using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.Domain.Models;
using FakeCSV.Domain.ViewModels;
using FakeCSV.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

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
        public IActionResult Index(int? id, bool? deleteColumnError = false)
        {
            if (deleteColumnError.GetValueOrDefault())
                ViewBag.ErrorMessage = "Delete failed. Try again.";

            if (id is null)
            {
                ViewBag.Title = "New Schema";
                return View(new NewSchemaViewModel());
            }

            var schema = dataService.GetSchemaById((int)id);
            var model = new NewSchemaViewModel
            {
                Id = schema.Id,
                Name = schema.Name,
                Separator = schema.Separator,
                Quotation = schema.Quotation,
                Columns = schema.Columns
                    .Select(c => new ColumnViewModel
                    {
                        ColumnName = c.Name,
                        LowerLimit = c.LowerLimit,
                        UpperLimit = c.UpperLimit,
                        Order = c.Order,
                        Type = c.Type,
                    })
                    .OrderBy(c => c.Order)
                    .ToList(),
            };

            ViewBag.Title = $"Edit Schema {model.Name}";
            return View(model);
        }

        [HttpPost]
        public IActionResult AddColumn(NewSchemaViewModel schemaModel)
        {
            ModelState.Clear();
            schemaModel.Columns.RemoveAll(c => c.IsDeleted == "deleted");
            var columnsList = schemaModel.Columns;
            var newColumn = new ColumnViewModel
            {
                ColumnName = schemaModel.AppendColumnName,
                Type = schemaModel.AppendColumnType,
                Order = schemaModel.AppendColumnOrder,
                LowerLimit = schemaModel.AppendColumnLowerLimit,
                UpperLimit = schemaModel.AppendColumnUpperLimit,
            };

            if (newColumn.Order <= columnsList.Count)
            {
                newColumn.Order = columnsList.Count + 1;
                var maxOrder = columnsList.Max(c => c.Order);
                if (newColumn.Order <= maxOrder)
                    newColumn.Order = maxOrder + 1;
            }

            columnsList.Add(newColumn);
            schemaModel.AppendColumnName = null;
            schemaModel.AppendColumnType = ColumnType.FullName;
            schemaModel.AppendColumnOrder = 1;
            schemaModel.AppendColumnUpperLimit = null;
            schemaModel.AppendColumnLowerLimit = null;

            return PartialView("Partial/_SchemaColumn", schemaModel);
        }

        [HttpPost]
        public IActionResult Submit(NewSchemaViewModel model)
        {

            if (!ModelState.IsValid)
                return View("Index", model);

            if (!model.Columns.Any())
            {
                ModelState.AddModelError("", "Schema must contain at least one column");
                return View("Index", model);
            }

            var schema = new Schema
            {
                Name = model.Name,
                Quotation = model.Quotation,
                Separator = model.Separator,
                CreationTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Columns = model.Columns.Where(c => c.IsDeleted != "deleted").Select(c => new Column
                {
                    Name = c.ColumnName,
                    Type = c.Type,
                    Order = c.Order,
                    LowerLimit = c.LowerLimit,
                    UpperLimit = c.UpperLimit,
                })
                    .ToList()
            };

            if (model.Id > 0)
            {
                schema.Id = model.Id;
                dataService.UpdateSchema(schema);
                return RedirectToAction("Index", "Home");
            }

            dataService.AddSchema(schema);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("Edit/DeleteColumn/{columnId:int}")]
        public IActionResult DeleteColumn(int columnId, NewSchemaViewModel model)
        {
            var column = model.Columns.FirstOrDefault(c => c.Id == columnId);

            if (column == null)
                return RedirectToAction("Index", new { id = model.Id, deleteColumnError = true });

            model.Columns.Remove(column);

            return PartialView("Partial/_SchemaColumn", model);
        }
    }
}
