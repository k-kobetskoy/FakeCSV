using FakeCSV.Domain.Models;
using System.Collections.Generic;

namespace FakeCSV.Services
{
    public interface ISchemaDataService
    {
        void AddSchema(Schema schema);
        void DeleteSchema(int id);
        Schema GetSchemaById(int id);
        IEnumerable<Schema> GetSchemas();
        void UpdateSchema(Schema newSchema);
    }
}