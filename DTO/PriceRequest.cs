using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace EIRLSSAssignment1.DTO
{
    [DataContract(Name = "PRICE_REQUEST")]
    public class PriceRequest
    {
        [DataMember(Name = "PR_VEHICLE_TYPE")]
        public string VehicleType { get; set; }
        [DataMember(Name = "PR_START_DATE")]
        public DateTime StartDate { get; set; }
        [DataMember(Name = "PR_END_DATE")]
        public DateTime EndDate { get; set; }
    }
}