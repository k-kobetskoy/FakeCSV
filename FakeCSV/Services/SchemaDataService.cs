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

        public void DeleteSchema(Schema schema)
        {
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

        public int AddDataSet(DataSet dataSet)
        {
            dbContext.Add(dataSet);
            dbContext.SaveChanges();
            return dataSet.Id;
        }

        public DataSet GetDatasetById(int id)
        {
            var dataSet = dbContext.DataSets.Find(id);
            return dataSet;
        }

      
    }
}
