using System.Threading.Tasks;

namespace FakeCSV.Services
{
    public interface IGenerateCsvService
    {
        Task<int> GenerateData(int schemaId, int rows);
    }
}