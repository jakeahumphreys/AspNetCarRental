using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.DAL;
using System.Data.Entity;
using System.IO;

namespace EIRLSSAssignment1.RepeatLogic
{
    public class Library
    {
        private ConfigurationRepository _configurationRepository;
        private ApplicationDbContext _applicationDbContext;
        private BookingRepository _bookingRepository;

        public Library()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _configurationRepository = new ConfigurationRepository(context);
            _applicationDbContext = new ApplicationDbContext();
            _bookingRepository = new BookingRepository(context);
        }
        public Configuration GetActiveConfiguration()
        {
            return _configurationRepository.GetConfigurations().Where(c => c.IsConfigurationActive == true).SingleOrDefault();
        }

        public bool IsGarageAllowingOrders()
        {
            Configuration config = GetActiveConfiguration();

            if(config != null)
            {
                return config.IsOpenForRentals;
            }
            else
            {
                return false;
            }
        }

        public int CalculateUserAge(string id)
        {
            ApplicationUser user = _applicationDbContext.Users.Find(id);
            
            if(user != null)
            {
                int age = 0;
                age = DateTime.Now.Year - user.DateOfBirth.Year;
                if(DateTime.Now.DayOfYear < user.DateOfBirth.DayOfYear)
                {
                    age = age - 1;
                }
                return age;
            }
            else
            {
                return 0;
            }
        }

        public bool IsUserBlacklisted(string id)
        {
            ApplicationUser user = _applicationDbContext.Users.Find(id);

            if (user != null)
            {
                return user.IsBlackListed;
            }
            else
            {
                return false;
            }
        }

        public bool CanUserReturnLate(string id)
        {
            ApplicationUser user = _applicationDbContext.Users.Find(id);

            if (user != null)
            {
                return user.IsTrustedCustomer;
            }
            else
            {
                return false;
            }
        }

        public bool isBookedNextDay(DateTime endDate, int vehicleId)
        {
            if(endDate != null)
            {
                var dateToCheck = endDate.AddDays(1);
                List<Booking> bookings = _bookingRepository.GetBookings().Where(x => x.VehicleId == vehicleId).Where(x => x.BookingStart.Day == dateToCheck.Day).ToList();
                if(bookings.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool isMinRental(DateTime startTime, DateTime endTime)
        {
            bool isMinRental = true;

            if(startTime != null && endTime != null)
            {
                Configuration config = GetActiveConfiguration();
                TimeSpan timeSpan = endTime.Subtract(startTime);

                if (timeSpan.TotalHours < config.MinRentalHours)
                {
                    isMinRental = false;
                }

            }

            return isMinRental;
        }

        public bool isMaxRental(DateTime startTime, DateTime endTime)
        {
            if (startTime != null && endTime != null)
            {
                Configuration config = GetActiveConfiguration();
                TimeSpan timeSpan = endTime.Subtract(startTime);

                if (timeSpan.TotalHours > config.MaxRentalHours)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool isBeyondClosing(int hour)
        {
            Configuration config = GetActiveConfiguration();

            if (hour > config.ClosingTime.Hour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isBeforeOpening(int hour)
        {
            Configuration config = GetActiveConfiguration();

            if (hour < config.OpeningTime.Hour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isWithinOpeningHours(int hour)
        {
            Configuration config = GetActiveConfiguration();

            if (hour > config.OpeningTime.Hour && hour < config.ClosingTime.Hour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void HandleAutoTrust(string userId)
        {
            Configuration config = GetActiveConfiguration();

            if(config != null)
            {
                if(config.LateReturnEligibility > 0)
                {
                    ApplicationUser user = _applicationDbContext.Users.Find(userId);

                    if(user != null)
                    {
                        List<Booking> bookingsForUser = _bookingRepository.GetBookings().Where(x => x.UserId == userId).Where(x => x.IsReturned == true).Where(x => x.ReturnDate <= x.BookingFinish).ToList();

                        if (bookingsForUser.Count >= config.LateReturnEligibility)
                        {

                            if(user.IsBlackListed == false && user.IsTrustedCustomer == false)
                            {
                                user.IsTrustedCustomer = true;
                                _applicationDbContext.Entry(user).State = EntityState.Modified;
                                _applicationDbContext.SaveChanges();
                            }
                        }
                    } 
                }
            }
        }

        private byte[] convertImageToByteArray(HttpPostedFileBase image)
        {
            byte[] imageByteArray = null;

            if (image != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.InputStream.CopyTo(memoryStream);
                    imageByteArray = memoryStream.GetBuffer();
                }
            }

            return imageByteArray;
        }
    }
}