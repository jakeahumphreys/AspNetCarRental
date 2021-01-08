using EIRLSSAssignment1.Customisations;
using EIRLSSAssignment1.DAL;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class ExtensionRequestService
    {
        private ExtensionRequestRepository _extensionRepository;
        private BookingRepository _bookingRepository;

        public ExtensionRequestService()
        {
            _extensionRepository = new ExtensionRequestRepository(new ApplicationDbContext());
            _bookingRepository = new BookingRepository(new ApplicationDbContext());
        }

        public ExtensionRequest GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("Expected integer");
            }
            ExtensionRequest extensionRequest = _extensionRepository.GetExtensionRequestById(id);
            if (extensionRequest == null)
            {
                throw new ExtensionRequestNotFoundException("Extension Request not found.");
            }
            return extensionRequest;
        }

        public ServiceResponse CreateAction(ExtensionRequest extension)
        {
            if (extension != null)
            {
                _extensionRepository.Insert(extension);
                _extensionRepository.Save();
                return new ServiceResponse { Result = true };
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public ServiceResponse ApproveExtension(bool approved, int extensionId)
        {
            ExtensionRequest extension = _extensionRepository.GetExtensionRequestById(extensionId);

            if (extension != null)
            {
                Booking booking = _bookingRepository.GetBookingById(extension.BookingId);

                if (booking != null)
                {
                    if (approved == true)
                    {
                        booking.BookingFinish = extension.EndDateRequest;
                        _bookingRepository.Update(booking);
                        _bookingRepository.Save();

                        extension.extensionRequestStatus = ExtensionStatus.Accepted;
                        _extensionRepository.Update(extension);
                        _extensionRepository.Save();

                        return new ServiceResponse { Result = true };
                    }
                    else
                    {
                        extension.extensionRequestStatus = ExtensionStatus.Rejected;
                        _extensionRepository.Update(extension);
                        _extensionRepository.Save();

                        return new ServiceResponse { Result = true };
                    }
                }
                else
                {
                    return new ServiceResponse { Result = false };
                }
            }
            else
            {
                return new ServiceResponse { Result = false };
            }
        }

        public void Dispose()
        {
            _extensionRepository.Dispose();
        }
    }
}