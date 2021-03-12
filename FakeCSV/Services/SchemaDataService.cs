using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeCSV.DAL.Context;
using FakeCSV.Data;
using FakeCSV.Domain.Models;
using FakeCSV.Domain.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FakeCSV.Services
{
    public class SchemaDataService : ISchemaDataService
    {
        private readonly FakeCsvDbContext dbContext;
        private readonly ILogger<SchemaDataService> logger;
        private readonly IWebHostEnvironment appEnvironment;

        public SchemaDataService(FakeCsvDbContext dbContext,
            ILogger<SchemaDataService> logger,
            IWebHostEnvironment appEnvironment)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.appEnvironment = appEnvironment;
        }

        public void AddSchema(Schema schema)
        {
            dbContext.Add(schema);
            dbContext.SaveChanges();
        }

        public IEnumerable<Schema> GetSchemas()
        {
            if (dbContext.Schemas.Any())
                return dbContext.Schemas
                    .Include(s => s.Columns)
                    .AsEnumerable();
            return null;
        }

        public Schema GetSchemaById(int id)
        {
            var schema = dbContext
                .Schemas
                .Include(s => s.Columns)
                .Include(d => d.DataSets)
                .FirstOrDefault(s => s.Id == id);
            return schema;
        }

        public void DeleteSchema(int id)
        {
            var schema = GetSchemaById(id);
            dbContext.Schemas.Remove(schema);
            dbContext.SaveChanges();
        }

        public void UpdateSchema(Schema newSchema)
        {
            var schema = GetSchemaById(newSchema.Id);
            schema.Columns = newSchema.Columns;
            schema.Name = newSchema.Name;
            schema.Quotation = newSchema.Quotation;
            schema.Separator = newSchema.Separator;
            schema.UpdateTime = DateTime.Now;
            dbContext.SaveChanges();
        }

        public IEnumerable<DataSet> GetDataSets(int id)
        {
            var schema = GetSchemaById(id);
            return schema.DataSets;
        }


        public async Task GenerateData(int schemaId, int rows)
        {
            var path = appEnvironment.WebRootPath + "/Files/";
            var fileName = $"{DateTime.Now:dd-MM-yy-hh-mm-ss}-{schemaId}-{rows:00000}.csv";

            var schema = GetSchemaById(schemaId);

            var separator = schema.Separator switch
            {
                ColumnSeparator.Comma => ",",
                ColumnSeparator.Semicolon => ";",
                ColumnSeparator.Tabulation => "\t",
                ColumnSeparator.Space => " ",
                _ => ","
            };

            var quotation = schema.Quotation switch
            {
                QuotationMark.BackQuote => "`",
                QuotationMark.DoubleQuote => "\"",
                QuotationMark.SiningleQuote => "\'",
                _ => "\""
            };

            var columns = schema.Columns.OrderBy(c => c.Order).ToList();
            var types = columns.Select(column => column.Type);






            await using (var fileStream = new FileStream(path + fileName, FileMode.CreateNew))
            {
                await using (var writer = new StreamWriter(fileStream))
                {
                    var header = string.Join(separator, columns.Select(c => c.Name));
                    writer.WriteLine(header);

                    var columnsData = GetData(columns, rows, quotation);
                    for (int i = 0; i < rows; i++)
                    {
                        string csvString = string.Empty;
                        foreach (var column in columnsData)
                            csvString = csvString + column[i] + separator;

                        await writer.WriteLineAsync(csvString);
                    }
                }


            }

        }

        private List<List<string>> GetData(List<Column> columns, int rows, string quotation)
        {
            List<List<string>> columnsData = new();
            columnsData = columns.Select(c => GetColumnData(c, rows, quotation)).ToList();
            return columnsData;
        }

        private List<string> GetColumnData(Column column, int rows, string quotation)
        {
            return column.Type switch
            {
                ColumnType.FullName => GetStrings(TestData.Names, rows),
                ColumnType.Job => GetStrings(TestData.Jobs, rows),
                ColumnType.Email => GetStrings(TestData.Emails, rows),
                ColumnType.DomainName => GetStrings(TestData.DomainNames, rows),
                ColumnType.PhoneNumber => GetStrings(TestData.PhoneNumbers, rows),
                ColumnType.CompanyName => GetStrings(TestData.CompanyNames, rows),
                ColumnType.Text => GetText(TestData.Sentences, rows, column.LowerLimit, column.UpperLimit, quotation),
                ColumnType.Integer => GetInteger(rows, column.LowerLimit, column.UpperLimit),
                ColumnType.Address => GetStrings(TestData.Addresses, rows),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private List<string> GetInteger(int rows, int? lowerLimit, int? upperLimit)
        {
            var r = new Random();
            var columnData = new List<string>();
            for (int i = 0; i < rows; i++)
                columnData.Add(r.Next((int)lowerLimit, (int)upperLimit).ToString());

            return columnData;
        }

        private List<string> GetText(string[] array, int rows, int? lowerLimit, int? upperLimit, string qutation)
        {
            var r = new Random();
            var columnData = new List<string>();
            for (int i = 0; i < rows; i++)
                columnData.Add(qutation
                               + string.Concat(array.Take(r.Next((int)lowerLimit, (int)upperLimit))
                                .OrderBy(s => r.Next(10)))
                               + qutation);

            return columnData;
        }

        private static List<string> GetStrings(string[] array, int rows)
        {

            var columnData = new List<string>();
            var r = new Random();

            if (array.Length >= rows)
            {
                columnData = array.Take(rows).OrderBy(o => r.Next(rows)).ToList();
                return columnData;
            }

            var iterations = rows / array.Length;
            var remainder = rows % array.Length;

            for (var i = 0; i < iterations; i++)
                columnData.AddRange(array.OrderBy(s => r.Next(rows)));
            columnData.AddRange(array.Take(remainder).OrderBy(o => r.Next(remainder)));
            return columnData;
        }
    }
}
