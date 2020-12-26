using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class DrivingLicenseRepository : IDrivingLicenseRepository
    {
        private readonly DrivingLicenseContext _context;

        public DrivingLicenseRepository(DrivingLicenseContext context)
        {
            _context = context;
        }

        public IList<DrivingLicense> GetDrivingLicenses()
        {
            return _context.DrivingLicenses.ToList();
        }

        public DrivingLicense GetDrivingLicenseById(int id)
        {
            return _context.DrivingLicenses.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(DrivingLicense DrivingLicense)
        {
            _context.DrivingLicenses.Add(DrivingLicense);
        }

        public void Update(DrivingLicense DrivingLicense)
        {
            _context.Entry(DrivingLicense).State = EntityState.Modified;
        }

        public void Delete(DrivingLicense DrivingLicense)
        {
            _context.DrivingLicenses.Remove(DrivingLicense);
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