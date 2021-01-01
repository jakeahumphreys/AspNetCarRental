using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class OptionalExtraRepository : IOptionalExtraRepository
    {
        private readonly OptionalExtraContext _context;

        public OptionalExtraRepository(OptionalExtraContext context)
        {
            _context = context;
        }

        public IList<OptionalExtra> GetOptionalExtras()
        {
            var optionalExtras = _context.OptionalExtras.Include(o => o.booking);
            return optionalExtras.ToList();
        }

        public OptionalExtra GetOptionalExtraById(int id)
        {
            return _context.OptionalExtras.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(OptionalExtra OptionalExtra)
        {
            _context.OptionalExtras.Add(OptionalExtra);
        }

        public void Update(OptionalExtra OptionalExtra)
        {
            _context.Entry(OptionalExtra).State = EntityState.Modified;
        }

        public void Delete(OptionalExtra OptionalExtra)
        {
            _context.OptionalExtras.Remove(OptionalExtra);
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