using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenId;
using DotNetOpenId.Extensions.SimpleRegistration;
using DotNetOpenId.RelyingParty;

namespace Bindable.Cms.Web.Controllers
{
    [HandleError]
    public class LoginController : Controller
    {
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Authenticate(string ReturnUrl, string openidIdentifier)
        {
            var provider = new OpenIDAuthenticationProvider() as IAuthenticationProvider;
            var result =  provider.Authenticate(openidIdentifier);
            if (result.Success)
            {
                FormsAuthentication.RedirectFromLoginPage(openidIdentifier, true);
                return new EmptyResult();
            }
            if (result.Cancelled)
            {
                ViewData["Message"] = "Your ";
                return View("Login");
            }
            if (result.Error != null)
            {
                ViewData["Message"] = "An error occurred during the authentication process: " + result.Error.Message;
                return View("Login");
            }
            ViewData["Message"] = "The authentication provider indicated that authentication has failed. Please try again.";
            return View("Login");
        }
    }

    public interface IAuthenticationProvider
    {
        AuthenticationResult Authenticate(string username);
    }

    public sealed class OpenIDAuthenticationProvider : IAuthenticationProvider
    {
        public AuthenticationResult Authenticate(string userIdentifier)
        {
            var openid = new OpenIdRelyingParty();
            if (openid.Response == null)
            {
                // Redirect the user to the provider party. They will login, and be redirected 
                // back here.
                try
                {
                    var claims = new ClaimsRequest();
                    claims.Nickname = DemandLevel.Require;
                    claims.FullName = DemandLevel.Require;
                    claims.Email = DemandLevel.Require;
                    claims.PostalCode = DemandLevel.Request;

                    var id = Identifier.Parse(userIdentifier);
                    var request = openid.CreateRequest(userIdentifier);
                    request.AddExtension(claims);
                    request.RedirectToProvider();
                }
                catch (OpenIdException ex)
                {
                    // The user may have entered an incorrectly formatted URI, the server is offline, etc.
                    return new AuthenticationResult() { Cancelled = false, Error = ex, Success = false, Username = userIdentifier };
                }
            }
            else
            {
                // The OpenID provider has processed the user's request and redirected them back here.
                switch (openid.Response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        // Update the member information
                        return new AuthenticationResult() {Cancelled = false, Error = null, Success = true, Username = userIdentifier};
                    case AuthenticationStatus.Canceled:
                        return new AuthenticationResult() {Cancelled = true, Error = null, Success = false, Username = userIdentifier};
                    case AuthenticationStatus.Failed:
                        return new AuthenticationResult() {Cancelled = false, Error = null, Success = false, Username = userIdentifier};
                }
            }
            return null;
        }
    }

    public class AuthenticationResult
    {
        public string Username { get; set; }
        public bool Cancelled { get; set; }
        public bool Success { get; set; }
        public Exception Error { get; set; }
    }
}
