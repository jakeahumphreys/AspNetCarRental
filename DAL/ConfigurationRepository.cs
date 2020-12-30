using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ConfigurationContext _context;

        public ConfigurationRepository(ConfigurationContext context)
        {
            _context = context;
        }

        public IList<Configuration> GetConfigurations()
        {
            return _context.Configurations.ToList();
        }

        public Configuration GetConfigurationById(int id)
        {
            return _context.Configurations.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(Configuration Configuration)
        {
            _context.Configurations.Add(Configuration);
        }

        public void Update(Configuration Configuration)
        {
            _context.Entry(Configuration).State = EntityState.Modified;
        }

        public void Delete(Configuration Configuration)
        {
            _context.Configurations.Remove(Configuration);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}