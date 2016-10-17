using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using API.Filters;
using API.Models.Common;
using Common.Helpers;
using Service.Interface.Business;
using Swashbuckle.Swagger.Annotations;

namespace API.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/common")]
    public class CommonController : BaseApiController
    {
        #region fields

        private readonly IYayYoApplicationService _yayYoApplicationService;

        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yayYoApplicationService"></param>
        public CommonController(IYayYoApplicationService yayYoApplicationService)
        {
            _yayYoApplicationService = yayYoApplicationService;
        }
        #endregion

        /// <summary>
        /// Get basic code
        /// </summary>
        /// <returns></returns>
        [Route("basiccode")]   
        [AllowNoBasicCode]     
        [HttpGet]
        [SwaggerResponse(200, "Returns the result of list basic code", typeof(List<BasicCodeModel>))]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public IHttpActionResult GetBasicCode()
        {
            var listApplications = _yayYoApplicationService.GetAllApplications();
            var listString = listApplications.Select(p =>
            {
                var resId = p.Id;
                var resEncryptSecret = p.EncryptSecret;
                var resDecryptSecret = CommonSecurityHelper.Decrypt(resEncryptSecret, CommonSecurityHelper.KeyEncrypt);
                var plainTextBytes = Encoding.UTF8.GetBytes(resId + ":" + resDecryptSecret);
                var res = Convert.ToBase64String(plainTextBytes);
                return new BasicCodeModel
                {
                    Name = p.Name,
                    Code = res
                };

            }).ToList();
            return Ok(listString.ToList());
        }
    }
}
