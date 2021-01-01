using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class BookingViewModel
    {
        public Booking booking { get; set; }
        [Display (Name = "Optional Extras")]
        public List<int> SelectedExtraIds { get; set; }
        [Display(Name = "Remove Extras")]
        public List<int> SelectedExtraToRemoveIds { get; set; }
        [Display(Name = "Add Extras")]
        public List<OptionalExtra> BookedOptionalExtras { get; set; }
    }
}