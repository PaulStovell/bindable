using System.Security.Principal;
using PaulStovell.Domain.Model;

namespace PaulStovell.Domain.Services
{
    public interface IAuthorizationContext
    {
        IPrincipal Principal { get;}
        Member CurrentMember { get;}
    }
}
