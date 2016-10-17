using Autofac.Integration.WebApi;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace API
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

      

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="resolver"></param>
        public void ConfigureAuth(IAppBuilder app, AutofacWebApiDependencyResolver resolver)
        {
            
        }
    }
}
