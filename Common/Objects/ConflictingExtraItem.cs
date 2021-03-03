using EIRLSSAssignment1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Common.Objects
{
    public class ConflictingExtraItem
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OptionalExtra OptionalExtra { get; set; }
    }
}