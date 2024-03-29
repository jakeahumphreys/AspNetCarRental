﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class BookingViewModel
    {
        //Utility
        public Booking booking { get; set; }
        [Display (Name = "Optional Extras")]
        public List<int> SelectedExtraIds { get; set; }
        [Display(Name = "Remove Extras")]
        public List<int> SelectedExtraToRemoveIds { get; set; }
        [Display(Name = "Add Extras")]
        public List<OptionalExtra> BookedOptionalExtras { get; set; }

        //Display
        [Required]
        [Display(Name ="Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }
        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }
    }
}