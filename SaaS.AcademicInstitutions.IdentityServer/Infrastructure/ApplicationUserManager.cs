using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SaaS.AcademicInstitutions.IdentityServer.Models;

namespace SaaS.AcademicInstitutions.IdentityServer.Infrastructure
{
    public class ApplicationUserManager : UserManager<AcademicUser>
    {
        public ApplicationUserManager(IUserStore<AcademicUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationIdentityContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<AcademicUser>(appDbContext));

            
            return appUserManager;
        }
    }
}