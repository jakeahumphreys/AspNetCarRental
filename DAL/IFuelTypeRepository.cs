
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IFuelTypeRepository : IDisposable
    {
        IList<FuelType> GetFuelTypes();
        FuelType GetFuelTypeById(int id);
        void Insert(FuelType fuelType);
        void Update(FuelType fuelType);
        void Delete(FuelType fuelType);
        void Save();
    }
}