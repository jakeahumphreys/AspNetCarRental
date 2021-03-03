using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace EIRLSSAssignment1.Models
{
    public class Configuration
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name="Opening Time")]
        public DateTime OpeningTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Closing Time")]
        public DateTime ClosingTime { get; set; }
        [Required]
        [Display(Name = "Open for Rentals?")]
        public bool IsOpenForRentals { get; set; }
        [Required]
        [Display(Name = "Auto LR Eligibility")]
        public int LateReturnEligibility { get; set; }
        [Required]
        [Display(Name = "Minimum Rental Period (Hours)")]
        public int MinRentalHours { get; set; }
        [Required]
        [Display(Name = "Maximum Rental Period (Hours)")]
        public int MaxRentalHours { get; set; }

        [Required]
        [Display(Name = "DVLA 15 Digit Reference")]
        [StringLength(15)]
        public string DvlaReference { get; set; }
        [Required]
        [Display(Name = "DVLA Import URL")]
        public string DvlaImportUrl { get; set; }
        [Required]
        [Display(Name = "ABI Import URL")]
        public string AbiImportUrl { get; set; }
        [Required]
        [Display(Name = "Price Check URL")]
        public string PriceCheckUrl { get; set; }

        [Display(Name = "Send Emails")]
        public bool SmtpShouldSendEmail { get; set; }
        [RequiredIfTrue("SmtpShouldSendEmail")]
        [Display(Name = "SMTP Server")]
        public string SmtpUrl { get; set; }
        [RequiredIfTrue("SmtpShouldSendEmail")]
        [Display(Name = "SMTP Port")]
        public int SmtpPort { get; set; }
        [RequiredIfTrue("SmtpShouldSendEmail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "SMTP Sender Email")]
        public string SmtpSenderEmail { get; set; }
        [RequiredIfTrue("SmtpShouldSendEmail")]
        [DataType(DataType.Password)]
        [Display(Name = "SMTP Sender Password")]
        public string SmtpSenderPassword { get; set; }
        [RequiredIfTrue("SmtpShouldSendEmail")]
        [Display(Name = "Use SSL")]
        public bool SmtpShouldUseSsl { get; set; }
        [RequiredIfTrue("SmtpShouldSendEmail")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "SMTP Recipient Email")]
        public string SmtpRecipientEmail { get; set; }

        [Display(Name ="Active?")]
        public bool IsConfigurationActive { get; set; }
    }
}