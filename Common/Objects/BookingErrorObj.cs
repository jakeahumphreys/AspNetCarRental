using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Common.Objects
{
    public class BookingErrorObj
    {
        public bool isBookedNextDay { get; set; }
        public bool isBelowMinRental{ get; set; }
        public bool isBeyondMaxRental{ get; set; }
        public bool isStartBeforeOpen { get; set; }
        public bool isStartAfterClose { get; set; }
        public bool isEndBeforeOpen { get; set; }
        public bool isEndAfterClose { get; set; }
        public bool isInThePast { get; set; }
        public bool isEndBeforeStart { get; set; }
        public bool isStartAfterEnd { get; set; }
        public List<Booking> conflictingBookings { get; set; }
        public List<ConflictingExtraItem> conflictingOptionalExtras { get; set; }

        public BookingErrorObj()
        {
            this.isBookedNextDay = false;
            this.isBelowMinRental = false;
            this.isBeyondMaxRental = false;
            this.isStartBeforeOpen = false;
            this.isStartAfterClose = false;
            this.isEndBeforeOpen = false;
            this.isEndAfterClose = false;
            this.isInThePast = false;
            this.isEndBeforeStart = false;
            this.isStartAfterEnd = false;
            this.conflictingBookings = null;
            this.conflictingOptionalExtras = null;
        }
    }
}