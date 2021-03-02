using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using EIRLSSAssignment1.RepeatLogic;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class ServiceResponse
    { 
        public bool Result { get; set; }
        public Object ServiceObject { get; set; }
        public ResponseError ResponseError { get; set; }
    }
}