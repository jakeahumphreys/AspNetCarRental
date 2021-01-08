using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class ExtensionRequestRepository : IExtensionRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ExtensionRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ExtensionRequest> GetExtensionRequests()
        {
            return _context.ExtensionRequests.Include(x => x.Booking).Include(x => x.Booking.Vehicle).ToList();
        }

        public ExtensionRequest GetExtensionRequestById(int id)
        {
            return _context.ExtensionRequests.Where(x => x.Id == id).Include(x => x.Booking).Include(x => x.Booking.Vehicle).SingleOrDefault();
        }

        public void Insert(ExtensionRequest ExtensionRequestRequest)
        {
            _context.ExtensionRequests.Add(ExtensionRequestRequest);
        }

        public void Update(ExtensionRequest ExtensionRequestRequest)
        {
            _context.Entry(ExtensionRequestRequest).State = EntityState.Modified;
        }

        public void Delete(ExtensionRequest ExtensionRequestRequest)
        {
            _context.ExtensionRequests.Remove(ExtensionRequestRequest);
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