using FakeCSV.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeCSV.Domain.ViewModels;

namespace FakeCSV.Services
{
    public interface ISchemaDataService
    {
        void AddSchema(Schema schema);
        void DeleteSchema(int id);
        Schema GetSchemaById(int id);
        IEnumerable<Schema> GetSchemas();
        void UpdateSchema(Schema newSchema);
        IEnumerable<DataSet> GetDataSets(int id);
        DataSet GetDatasetById(int id);
        Task<int> GenerateData(int schemaId, int rows);
    }
}