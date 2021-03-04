using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.Data;
using FakeCSV.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using FakeCSV.Domain.Models;

namespace FakeCSV.Controllers
{
    [Authorize]
    public class NewSchemaController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var model = new NewSchemaViewModel();
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
            return View("Index", model);
        }


        [HttpPost]
        public IActionResult Submit(NewSchemaViewModel model)
        {
            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Delete(NewSchemaViewModel model, int deleteid)
        {
            model.Columns.RemoveAt(deleteid);

            model.AddColumnUpperLimit = 0;
            model.AddColumnName = null;
            model.AddColumnOrder = 0;
            model.AddColumnType = 0;
            model.AddColumnLowerLimit = 0;

            ModelState.Clear();
            return View("Index", model);
        }

    }
}
