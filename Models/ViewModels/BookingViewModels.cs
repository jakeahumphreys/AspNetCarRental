﻿using EIRLSSAssignment1.RepeatLogic.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1.Models.ViewModels
{
    public class BookingCreateViewModel
    {
        public SelectList Vehicles { get; set; }
        public MultiSelectList OptionalExtras { get; set; }
        public SelectList Users { get; set; }
        public int VehicleCount { get; set; }
        public int OptionalExtraCount { get; set; }
        public int BookedOptionalExtraCount { get; set; }
        public bool IsTrustedUser { get; set; }
        public Booking booking { get; set; }

        public BookingErrorObj ErrorObj { get; set; }

        public List<ConflictingExtraItem> ConflictingOptionalExtras { get; set; }

        public bool hasPendingRequest { get; set; }

        [Display(Name = "Optional Extras")]
        public List<int> SelectedExtraIds { get; set; }
        [Display(Name = "Remove Extras")]
        public List<int> SelectedExtraToRemoveIds { get; set; }
        [Display(Name = "Add Extras")]
        public MultiSelectList BookedOptionalExtras { get; set; }

        //Display
        [Required]
        [Display(Name = "Start Date")]
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

    public class BookingReturnViewModel
    { 
        public Booking booking { get; set; }
        [Display(Name = "Date Status")]
        public string dateStatus { get; set; }    
    }

}