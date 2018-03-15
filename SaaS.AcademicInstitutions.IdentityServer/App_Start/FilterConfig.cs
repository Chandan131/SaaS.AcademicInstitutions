using System.Web;
using System.Web.Mvc;

namespace SaaS.AcademicInstitutions.IdentityServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
