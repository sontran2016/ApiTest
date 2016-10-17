using System.Web.Http;
using API.Filters;

namespace API.Controllers
{
    /// <summary>
    /// base api 
    /// </summary>

    /// <summary>
    ///  Base API Controller
    /// </summary>
    [AllowAnonymous]
    [ValidateBasicAuthentication]
    public class BaseApiController : ApiController
    {
        
    }

    /// <summary>
    /// Success Message
    /// </summary>
    public class SuccessMessage
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
    }
}