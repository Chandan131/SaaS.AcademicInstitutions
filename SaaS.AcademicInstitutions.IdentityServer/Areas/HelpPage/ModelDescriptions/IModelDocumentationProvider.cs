using System;
using System.Reflection;

namespace SaaS.AcademicInstitutions.IdentityServer.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}