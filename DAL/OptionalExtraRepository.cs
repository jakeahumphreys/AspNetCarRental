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
        private readonly ApplicationDbContext _context;

        public OptionalExtraRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<OptionalExtra> GetOptionalExtras()
        {
            return _context.OptionalExtras.Include(x => x.Bookings).ToList();
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

        public void Attach(OptionalExtra optionalExtra)
        {
            _context.OptionalExtras.Attach(optionalExtra);
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