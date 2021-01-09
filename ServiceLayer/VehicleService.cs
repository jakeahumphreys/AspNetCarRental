using EIRLSSAssignment1.Customisations;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class VehicleService
    {
        private VehicleRepository _vehicleRepository;
        private FuelTypeRepository _fuelTypeRepository;
        private VehicleTypeRepository _vehicleTypeRepository;

        public VehicleService()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _vehicleRepository = new VehicleRepository(context);
            _fuelTypeRepository = new FuelTypeRepository(context);
            _vehicleTypeRepository = new VehicleTypeRepository(context);
        }

        public Vehicle GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException("Vehicle not found.");
            }
            return vehicle;
        }

        public VehicleViewModel CreateView()
        {
            VehicleViewModel vehicleVM = new VehicleViewModel
            {
                FuelTypes = new SelectList(_fuelTypeRepository.GetFuelTypes().Where(x => x.IsInactive == false), "Id", "Value"),
                VehicleTypes = new SelectList(_vehicleTypeRepository.GetVehicleTypes().Where(x => x.IsInactive == false), "Id", "Value")   
            };

            return vehicleVM;
        }

        public ServiceResponse CreateAction(VehicleViewModel vehicleVM)
        {
            if(vehicleVM != null)
            {
                _vehicleRepository.Insert(vehicleVM.vehicle);
                _vehicleRepository.Save();
                return new ServiceResponse { Result = true, ServiceObject = null };
            }
            else
            {
                return new ServiceResponse { Result = false, ServiceObject = vehicleVM };
            }

        }

        public VehicleViewModel EditView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException("Vehicle not found");
            }

            VehicleViewModel vehicleVM = new VehicleViewModel
            {
                vehicle = vehicle,
                FuelTypes = new SelectList(_fuelTypeRepository.GetFuelTypes().Where(x => x.IsInactive == false), "Id", "Value"),
                VehicleTypes = new SelectList(_vehicleTypeRepository.GetVehicleTypes().Where(x => x.IsInactive == false), "Id", "Value")
            };

            return vehicleVM;
        }

        public ServiceResponse EditAction(VehicleViewModel vehicleVM)
        {
            if(vehicleVM != null)
            {
                _vehicleRepository.Update(vehicleVM.vehicle);
                _vehicleRepository.Save();
                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false, ServiceObject = vehicleVM };
            }
        }

        public Vehicle DeleteView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException("Vehicle not found");
            }
            return vehicle;
        }

        public ServiceResponse DeleteAction(int id)
        {
            if(id != 0)
            {
                Vehicle vehicle = _vehicleRepository.GetVehicleById(id);
                _vehicleRepository.Delete(vehicle);
                _vehicleRepository.Save();
                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public void Dispose()
        {
            _vehicleRepository.Dispose();
        }
    }

    
   
}