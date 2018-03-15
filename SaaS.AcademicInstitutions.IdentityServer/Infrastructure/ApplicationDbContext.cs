using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SaaS.AcademicInstitutions.IdentityServer.Models;

namespace SaaS.AcademicInstitutions.IdentityServer.Infrastructure
{
    public class ApplicationIdentityContext : IdentityDbContext<AcademicUser>
    {
        public ApplicationIdentityContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationIdentityContext Create()
        {
            return new ApplicationIdentityContext();
        }
       
    }

    
}