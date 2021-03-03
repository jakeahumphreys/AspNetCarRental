using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class VehicleTypeService
    {
        private VehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeService()
        {
            _vehicleTypeRepository = new VehicleTypeRepository(new ApplicationDbContext());
        }

        public VehicleType GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            VehicleType vehicleType = _vehicleTypeRepository.GetVehicleTypeById(id);
            if (vehicleType == null)
            {
                throw new VehicleTypeNotFoundException("Fuel Type not found.");
            }
            return vehicleType;
        }

        public ServiceResponse CreateAction(VehicleType vehicleType)
        {
            if (vehicleType != null)
            {
                _vehicleTypeRepository.Insert(vehicleType);
                _vehicleTypeRepository.Save();

                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public VehicleType EditView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            VehicleType vehicleType = _vehicleTypeRepository.GetVehicleTypeById(id);
            if (vehicleType == null)
            {
                throw new VehicleTypeNotFoundException("Fuel Type not found");
            }
            return vehicleType;
        }

        public ServiceResponse EditAction(VehicleType vehicleType)
        {
            if (vehicleType != null)
            {
                _vehicleTypeRepository.Update(vehicleType);
                _vehicleTypeRepository.Save();

                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public VehicleType DeleteView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            VehicleType vehicleType = _vehicleTypeRepository.GetVehicleTypeById(id);
            if (vehicleType == null)
            {
                throw new VehicleTypeNotFoundException("Fuel Type Not Found");
            }
            return vehicleType;
        }

        public ServiceResponse DeleteAction(int id)
        {
            if (id != 0)
            {
                VehicleType vehicleType = _vehicleTypeRepository.GetVehicleTypeById(id);
                _vehicleTypeRepository.Delete(vehicleType);
                _vehicleTypeRepository.Save();
                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }

        }

        public void Dispose()
        {
            _vehicleTypeRepository.Dispose();
        }
    }
}