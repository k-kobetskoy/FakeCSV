using System.Collections.Generic;

namespace FakeCSV.Domain.Models
{
    public class Schema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ColumnSeparator Separator { get; set; }
        public QuotationMark Quotation { get; set; }
        public ICollection<Column> Columns { get; set; }

    }
}
