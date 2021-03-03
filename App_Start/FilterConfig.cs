using EIRLSSAssignment1.Common;
using System.Web;
using System.Web.Mvc;

namespace EIRLSSAssignment1
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
