using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class Success: IHttpActionResult
    {
        private readonly HttpStatusCode _statusCode;
        readonly object _responseData;
        /// <summary>
        /// 
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="responseData"></param>
        /// <param name="statusCode"></param>
        public Success(ApiController controller, object responseData, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            this._statusCode = statusCode;

            if (responseData is string)
            {
                _responseData = new
                {
                    Message = responseData
                };
            }
            else
            {
                this._responseData = responseData;
            }
            
            this.Request = controller.Request;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = Request.CreateResponse(this._statusCode, this._responseData, JsonMediaTypeFormatter.DefaultMediaType);
            return Task.FromResult(response);
        }
    }
}
