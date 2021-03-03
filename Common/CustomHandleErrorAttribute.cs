using EIRLSSAssignment1.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EIRLSSAssignment1.Common
{
    public class CustomHandleErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Error", ErrorType = ErrorType.System, Message = filterContext.Exception.Message }));
        }
    }
}