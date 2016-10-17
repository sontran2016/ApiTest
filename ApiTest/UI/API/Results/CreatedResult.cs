using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Results
{
    /// <summary>
    /// Created Result, usually used for POST
    /// </summary>
    public class Created: IHttpActionResult
    {
        private HttpStatusCode _statusCode = HttpStatusCode.Created;
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
        public Created(ApiController controller, object responseData)
        {
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
            var response = Request.CreateResponse(this._statusCode, this._responseData);
            return Task.FromResult(response);
        }
    }
}
