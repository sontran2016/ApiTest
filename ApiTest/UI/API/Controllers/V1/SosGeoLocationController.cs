using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using API.Models.SosGeoLocation;
using Core.Domain.Business;
using Service.Interface.Business;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;

namespace API.Controllers.V1
{
    /// <summary>
    /// sosGeoLocation
    /// </summary>
    [RoutePrefix("api/v1/logsos")]
    public class SosGeoLocationController : BaseApiController
    {
        #region fields

        private readonly ISosGeolocationService _sosGeoLocationService;
        private readonly ILogSosService _logSosService;
        #endregion

        #region ctor

        /// <summary>
        /// SosGeoLocation Controller
        /// </summary>
        /// <param name="sosGeoLocationService"></param>
        /// <param name="logSosService"></param>
        public SosGeoLocationController(ISosGeolocationService sosGeoLocationService, ILogSosService logSosService)
        {
            _sosGeoLocationService = sosGeoLocationService;
            _logSosService = logSosService;
        }

        #endregion

        /// <summary>
        /// Insert Sos Location
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.<br />
        /// <b>Description</b>: Timestamp must be from a DateTime UTC value
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerResponse(200, "Returns the result of create SosGeoLocation")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> InsertListSosGeoLocation(CreateListSosGeoLocationModel model)
        {
            try
            {
                //validate
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                    {
                        return BadRequest(ModelState[key].Errors[0].ErrorMessage);
                    }
                }
                //find LogSOS info
                var yayYoId = model.YayYoId;
                var logSos = await _logSosService.GetLastestLogSosAsync(yayYoId);
                if (logSos != null)
                {
                    var list = new List<LogSosGeolocation>();
                    foreach (var location in model.Locations)
                    {
                        var createdOnUtc = DateTimeOffset.FromUnixTimeSeconds(location.Timestamp).DateTime;
                        var sosGeoLocation = new LogSosGeolocation
                        {
                            LogSosId = logSos.Id,
                            Location = location.Location,
                            CreatedOnUtc = createdOnUtc
                        };
                        list.Add(sosGeoLocation);
                    }
                    await _sosGeoLocationService.InsertRangeAsync(list);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}