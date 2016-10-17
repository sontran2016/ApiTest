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
    public class Deleted: IHttpActionResult
    {
        private HttpStatusCode _statusCode = HttpStatusCode.NoContent;

        /// <summary>
        /// 
        /// </summary>
        public HttpRequestMessage Request { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public Deleted(ApiController controller)
        {
            this.Request = controller.Request;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = Request.CreateResponse(this._statusCode);
            return Task.FromResult(response);
        }
    }
}
