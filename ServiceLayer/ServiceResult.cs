using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.ServiceLayer
{
    public class ServiceResult<T>
    {
        public T ResultObject { get; set; }
    }
}