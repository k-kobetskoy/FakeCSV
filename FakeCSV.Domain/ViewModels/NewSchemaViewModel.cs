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
        
        #region AddingColumns
        [Display(Name = "Column name")]
        public string AddColumnName { get; set; }
        [Display(Name = "Column type")]
        public ColumnType AddColumnType { get; set; }
        [Display(Name = "Order")]
        public int AddColumnOrder { get; set; }
        [Display(Name = "Lower limit")]
        public int AddColumnLowerLimit { get; set; }
        [Display(Name = "Upper limit")]
        public int AddColumnUpperLimit { get; set; }
        #endregion

        public List<ColumnViewModel> Columns { get; set; }



    }
}
