using System.Collections.Generic;
using Hangfire.Dashboard;

namespace API.Helpers
{
    /// <summary>
    /// MyRestrictiveAuthorizationFilter
    /// </summary>
#pragma warning disable 618
    public class MyRestrictiveAuthorizationFilter : IAuthorizationFilter
#pragma warning restore 618
    {
        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="owinEnvironment"></param>
        /// <returns></returns>
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            // In case you need an OWIN context, use the next line,
            // `OwinContext` class is the part of the `Microsoft.Owin` package.
            //var context = new OwinContext(owinEnvironment);

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return context.Authentication.User.Identity.IsAuthenticated;
            return true;
        }
    }
}