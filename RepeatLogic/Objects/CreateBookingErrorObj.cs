using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.RepeatLogic.Objects
{
    public class CreateBookingErrorObj
    {
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

        public CreateBookingErrorObj()
        {
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