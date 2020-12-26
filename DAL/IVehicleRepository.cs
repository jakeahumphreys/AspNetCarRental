using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IVehicleRepository : IDisposable
    {
        IList<Vehicle> GetVehicles();
        Vehicle GetVehicleById(int id);
        void Insert(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);
        void Save();
    }
}