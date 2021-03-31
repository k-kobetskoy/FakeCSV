using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeCSV.Data;

using FakeCSV.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FakeCSV.Domain.ViewModels
{
    public class NewSchemaViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Schema name")]
        public string Name { get; set; }

        [Display(Name = "Column separator")]
        [EnumDataType(typeof(ColumnSeparator))]
        public ColumnSeparator Separator { get; set; }

        [Display(Name = "String character")]
        [EnumDataType(typeof(QuotationMark))]
        public QuotationMark Quotation { get; set; }
        public List<ColumnViewModel> Columns { get; set; }

        public string AppendColumnName { get; set; }
        public ColumnType AppendColumnType { get; set; }
        public int AppendColumnOrder { get; set; }
        public int? AppendColumnLowerLimit { get; set; }
        public int? AppendColumnUpperLimit { get; set; }
        

        //public ColumnViewModel ColumnToAdd { get; set; }

        public NewSchemaViewModel()
        {
            Columns = new List<ColumnViewModel>();
        }
    }
}
