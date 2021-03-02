using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRLSSAssignment1.RepeatLogic
{
    public enum ResponseError
    {
        InvalidParameter = 0,
        NullParameter = 1,
        EntityNotFound = 2,
        ValidationFailed= 3
    }
}