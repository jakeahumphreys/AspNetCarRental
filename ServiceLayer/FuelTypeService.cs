using EIRLSSAssignment1.Common;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class FuelTypeService
    {
        private FuelTypeRepository _fuelTypeRepository;

        public FuelTypeService()
        {
            _fuelTypeRepository = new FuelTypeRepository(new ApplicationDbContext());
        }

        public FuelType GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            if (fuelType == null)
            {
                throw new FuelTypeNotFoundException("Fuel Type not found.");
            }
            return fuelType;
        }

        public ServiceResponse CreateAction(FuelType fuelType)
        {
            if (fuelType != null)
            {
                _fuelTypeRepository.Insert(fuelType);
                _fuelTypeRepository.Save();

                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public FuelType EditView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            if (fuelType == null)
            {
                throw new FuelTypeNotFoundException("Fuel Type not found");
            }
            return fuelType;
        }

        public ServiceResponse EditAction(FuelType fuelType)
        {
            if(fuelType != null)
            {
                _fuelTypeRepository.Update(fuelType);
                _fuelTypeRepository.Save();

                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public FuelType DeleteView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
            if (fuelType == null)
            {
                throw new FuelTypeNotFoundException("Fuel Type Not Found");
            }
            return fuelType;
        }

        public ServiceResponse DeleteAction(int id)
        {
            if(id != 0)
            {
                FuelType fuelType = _fuelTypeRepository.GetFuelTypeById(id);
                _fuelTypeRepository.Delete(fuelType);
                _fuelTypeRepository.Save();
                return new ServiceResponse { Result = true };                  
            }
            else
            {
                return new ServiceResponse { Result = false };
            }

        }

        public void Dispose()
        {
            _fuelTypeRepository.Dispose();
        }
    }
}