using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeCSV.Domain.ViewModels
{
   public class SchemaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
