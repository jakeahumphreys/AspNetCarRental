using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Common.Objects
{
    public class EditBookingErrorObj
    {

        public bool isBeyondPeriod { get; set; }
        public bool isBookedNextDay { get; set; }
        public bool isBeyondClose { get; set; }
        public bool isBeforeOpen { get; set; }

        public EditBookingErrorObj()
        {
            this.isBeyondPeriod = false;
            this.isBookedNextDay = false;
            this.isBeyondClose = false;
            this.isBeforeOpen = false;
        }
       
    }
}