using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeCSV.Domain.Models
{
    public class Column
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ColumnType Type { get; set; }
        public Schema Schema { get; set; }
        public int Order { get; set; }
        public int? LowerLimit { get; set; }
        public int? UpperLimit { get; set; }
    }
}
