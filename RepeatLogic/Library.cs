using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIRLSSAssignment1.Models;
using EIRLSSAssignment1.DAL;

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

        public bool isBookedNextDay(DateTime date, int vehicleId)
        {
            if(date != null)
            {
                var dateToCheck = date.AddDays(1);
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
            if(startTime != null && endTime != null)
            {
                Configuration config = GetActiveConfiguration();
                TimeSpan timeSpan = endTime.Subtract(startTime);

                if (timeSpan.TotalHours > config.MinRentalHours)
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
    }
}