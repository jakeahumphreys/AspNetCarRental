using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class FuelTypeRepository : IFuelTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public FuelTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<FuelType> GetFuelTypes()
        {
            return _context.FuelTypes.ToList();
        }

        public FuelType GetFuelTypeById(int id)
        {
            return _context.FuelTypes.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(FuelType FuelType)
        {
            _context.FuelTypes.Add(FuelType);
        }

        public void Update(FuelType FuelType)
        {
            _context.Entry(FuelType).State = EntityState.Modified;
        }

        public void Delete(FuelType FuelType)
        {
            _context.FuelTypes.Remove(FuelType);
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