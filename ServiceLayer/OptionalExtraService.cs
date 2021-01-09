using EIRLSSAssignment1.Customisations;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class OptionalExtraService
    {
        private OptionalExtraRepository _optionalExtraRepository;

        public OptionalExtraService()
        {
            _optionalExtraRepository = new OptionalExtraRepository(new ApplicationDbContext());
        }

        public OptionalExtra GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            if (optionalExtra == null)
            {
                throw new OptionalExtraNotFoundException("Fuel Type not found.");
            }
            return optionalExtra;
        }

        public ServiceResponse CreateAction(OptionalExtra optionalExtra)
        {
            if (optionalExtra != null)
            {
                _optionalExtraRepository.Insert(optionalExtra);
                _optionalExtraRepository.Save();

                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public OptionalExtra EditView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            if (optionalExtra == null)
            {
                throw new OptionalExtraNotFoundException("Fuel Type not found");
            }
            return optionalExtra;
        }

        public ServiceResponse EditAction(OptionalExtra optionalExtra)
        {
            if (optionalExtra != null)
            {
                _optionalExtraRepository.Update(optionalExtra);
                _optionalExtraRepository.Save();

                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public OptionalExtra DeleteView(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
            if (optionalExtra == null)
            {
                throw new OptionalExtraNotFoundException("Fuel Type Not Found");
            }
            return optionalExtra;
        }

        public ServiceResponse DeleteAction(int id)
        {
            if (id != 0)
            {
                OptionalExtra optionalExtra = _optionalExtraRepository.GetOptionalExtraById(id);
                _optionalExtraRepository.Delete(optionalExtra);
                _optionalExtraRepository.Save();
                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }

        }

        public void Dispose()
        {
            _optionalExtraRepository.Dispose();
        }
    }
}