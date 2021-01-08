using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Customisations
{
    public class UserIsBlacklistedException : Exception
    {
        public UserIsBlacklistedException(string message) : base(message) { }
    }

    public class GarageIsClosedException : Exception
    {
        public GarageIsClosedException(string message) : base(message) { }
    }

    public class ParameterNotValidException : Exception
    {
        public ParameterNotValidException(string message) : base(message) { }
    }

    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message) : base(message) { }
    }


}