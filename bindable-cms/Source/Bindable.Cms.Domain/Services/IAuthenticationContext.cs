using System.Security.Principal;
using Bindable.Cms.Domain.Model;

namespace Bindable.Cms.Domain.Services
{
    public interface IAuthorizationContext
    {
        IPrincipal Principal { get;}
        Member CurrentMember { get;}
    }
}
