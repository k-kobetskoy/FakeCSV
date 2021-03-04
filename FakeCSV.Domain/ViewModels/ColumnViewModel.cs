using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeCSV.Data;
using FakeCSV.Domain.Models;

namespace FakeCSV.Domain.ViewModels
{
    public class ColumnViewModel
    {
        [Required]
        [Display(Name = "Column name")]
        [MaxLength(20)]
        public string ColumnName { get; set; }


        [Display(Name = "Column type")]
        public ColumnType Type { get; set; }


        [Display(Name = "Order")]
        public int Order { get; set; }

        [Display(Name = "Lower limit")]
        public int? LowerLimit { get; set; }
        [Display(Name = "Upper limit")]
        public int? UpperLimit { get; set; }

        
    }
}