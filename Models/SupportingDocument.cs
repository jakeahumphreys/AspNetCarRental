using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.Models
{
    public class AbiFraudulentClaim
    {
        public int Id { get; set; }
        public string FamilyName { get; set; }
        public string Forenames { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressOfClaim { get; set; }
        public DateTime DateOfClaim { get; set; }
        public string InsurerCode { get; set; }
    }
}