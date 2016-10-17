using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace API.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class Error : IHttpActionResult
    {
        private readonly HttpStatusCode _statusCode;
        readonly object _responseData;
        /// <summary>
        /// Request
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="responseData"></param>
        /// <param name="statusCode"></param>
        public Error(ApiController controller, object responseData, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            _statusCode = statusCode;

            if (responseData is string)
            {
                _responseData = new
                {
                    ErrorMessage = responseData
                };
            }
            else
            {
                _responseData = responseData;
            }

            Request = controller.Request;
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="exception"></param>
        public Error(ApiController controller, Exception exception)
        {
            _statusCode = HttpStatusCode.BadRequest;
            Request = controller.Request;
            _responseData = exception;
        }
        /// <summary>
        /// Error
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="exception"></param>
        /// <param name="statusCode"></param>
        public Error(ApiController controller, Exception exception, HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
            Request = controller.Request;
            _responseData = exception;
        }
        /// <summary>
        /// ExecuteAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = Request.CreateResponse(_statusCode, _responseData);
            return Task.FromResult(response);
        }
    }
}
