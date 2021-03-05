using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.DAL.Context;
using FakeCSV.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace FakeCSV.Services
{
    public class SchemaDataService : ISchemaDataService
    {
        private readonly FakeCsvDbContext dbContext;
        private readonly ILogger<SchemaDataService> logger;

        public SchemaDataService(FakeCsvDbContext dbContext, ILogger<SchemaDataService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
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
            schema.UpdateTime=DateTime.Now;
            dbContext.SaveChanges();
        }


        //public Column AddColumn()
        //{

        //}

        //public void UpdateColumn()
        //{

        //}

        //public void DeleteColumn()
        //{

        //}
    }
}
