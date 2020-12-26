
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IVehicleTypeRepository : IDisposable
    {
        IList<VehicleType> GetVehicleTypes();
        VehicleType GetVehicleTypeById(int id);
        void Insert(VehicleType VehicleType);
        void Update(VehicleType VehicleType);
        void Delete(VehicleType VehicleType);
        void Save();
    }
}