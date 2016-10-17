using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.Owin;
using Common.Helpers;
using Service.Interface.Business;

namespace API.Filters
{
    /// <summary>
    /// Validate basic authentication
    /// </summary>
    public class ValidateBasicAuthenticationAttribute : ActionFilterAttribute
    {
        private const string Allowanonymousattribute = "AllowNoBasicCodeAttribute";

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);
            if (
                ((ReflectedHttpActionDescriptor) actionContext.ActionDescriptor).MethodInfo.CustomAttributes.Any(
                    t => t.AttributeType.Name == Allowanonymousattribute))
            {
                return true;
            }

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowNoBasicCodeAttribute>().Any()
                   ||
                   actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowNoBasicCodeAttribute>()
                       .Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }
            //get client id and client secret key
            var headers = actionContext.Request.Headers;
            var authHeader = headers.Authorization;
            if (authHeader == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.Unauthorized,
                    new
                    {
                        message = "You do not have permission for execute this action"
                    },
                    actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                    );
            }
            else
            {
                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeader.Scheme.Equals("basic",
                    StringComparison.OrdinalIgnoreCase) &&
                    authHeader.Parameter != null)
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    try
                    {
                        var credentials = encoding.GetString(Convert.FromBase64String(authHeader.Parameter));

                        int separator = credentials.IndexOf(':');
                        string clientAppId = credentials.Substring(0, separator);
                        string clientAppSecret = credentials.Substring(separator + 1);

                        var owinContext = actionContext.Request.GetOwinContext();
                        var scopes = owinContext.GetAutofacLifetimeScope();
                        var yayYoApplicationService = scopes.Resolve<IYayYoApplicationService>();
                        var application = yayYoApplicationService.GetById(Convert.ToInt32(clientAppId));
                        if (application == null)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(
                                HttpStatusCode.Unauthorized,
                                new
                                {
                                    message = "You do not have permission for execute this action"
                                },
                                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                                );
                        }
                        else
                        {
                            if (application.AppSecret != CommonSecurityHelper.GetHash(clientAppSecret))
                            {

                                actionContext.Response = actionContext.Request.CreateResponse(
                                    HttpStatusCode.Unauthorized,
                                    new
                                    {
                                        message = "You do not have permission for execute this action"
                                    },
                                    actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                                    );
                            }
                        }
                    }
                    catch (Exception)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(
                            HttpStatusCode.Unauthorized,
                            new
                            {
                                message = "You do not have permission for execute this action"
                            },
                            actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                            );
                    }

                }
            }
        }
    }
}