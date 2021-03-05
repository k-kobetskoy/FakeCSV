using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeCSV.Domain.Models
{
    public class Schema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ColumnSeparator Separator { get; set; }
        public QuotationMark Quotation { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreationTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        public ICollection<Column> Columns { get; set; }


    }
}
