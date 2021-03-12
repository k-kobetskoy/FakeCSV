using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeCSV.Domain.Models
{
    public class DataSet
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        
        //name in filesystem 
        public string Name { get; set; }
        public Schema Schema { get; set; }
        public int RowsNumber { get; set; }

    }
}
