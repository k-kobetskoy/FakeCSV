using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FakeCSV.Data;

namespace FakeCSV.Domain.ViewModels
{
    public class DataSetsPageViewModel
    {
        public IEnumerable<DataSetViewModel> DataSets { get; set; }
        public string SchemaName { get; set; }
        public int SchemaId { get; set; }
        
        [Required][Range(1, 10000, ErrorMessage = "Set value beetwen 1 and 10000")]
        public int RowsNumber { get; set; }

    }
}