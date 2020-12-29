using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace EIRLSSAssignment1.DAL
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleContext _context;

        public VehicleRepository(VehicleContext context)
        {
            _context = context;
        }

        public IList<Vehicle> GetVehicles()
        {
            var vehicles = _context.Vehicles.Include(v => v.FuelType).Include(v => v.VehicleType).ToList();
            return vehicles;
        }

        public Vehicle GetVehicleById(int id)
        {
            return _context.Vehicles.Where(x => x.Id == id).Include(v => v.FuelType).Include(v => v.VehicleType).SingleOrDefault();
        }

        public void Insert(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Update(Vehicle vehicle)
        {
            _context.Entry(vehicle).State = EntityState.Modified;
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
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