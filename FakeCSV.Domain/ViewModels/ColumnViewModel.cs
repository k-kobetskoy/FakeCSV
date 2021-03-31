using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeCSV.Data;
using FakeCSV.Domain.Models;
using Microsoft.AspNetCore.Mvc;



namespace FakeCSV.Domain.ViewModels
{
    public class ColumnViewModel
    {
        [Required]
        [Display(Name = "Column name")]
        [MaxLength(50)]
        public string ColumnName { get; set; }

        [Display(Name = "Column type")]
        public ColumnType Type { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Can not be lower than 1")]
        [Display(Name = "Order")]
        public int Order { get; set; }

        [Display(Name = "Lower limit")]
        public int? LowerLimit { get; set; }
        [Display(Name = "Upper limit")]
        public int? UpperLimit { get; set; }
        public string IsDeleted { get; set; }


    }
}