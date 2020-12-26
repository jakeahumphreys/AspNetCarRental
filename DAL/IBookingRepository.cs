using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.DAL
{
    public interface IBookingRepository : IDisposable
    {
        IList<Booking> GetBookings();
        Booking GetBookingById(int id);
        void Insert(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
        void Save();
    }
}