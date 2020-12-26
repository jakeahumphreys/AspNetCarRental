using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly VehicleTypeContext _context;

        public VehicleTypeRepository(VehicleTypeContext context)
        {
            _context = context;
        }

        public IList<VehicleType> GetVehicleTypes()
        {
            return _context.VehicleTypes.ToList();
        }

        public VehicleType GetVehicleTypeById(int id)
        {
            return _context.VehicleTypes.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(VehicleType VehicleType)
        {
            _context.VehicleTypes.Add(VehicleType);
        }

        public void Update(VehicleType VehicleType)
        {
            _context.Entry(VehicleType).State = EntityState.Modified;
        }

        public void Delete(VehicleType VehicleType)
        {
            _context.VehicleTypes.Remove(VehicleType);
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