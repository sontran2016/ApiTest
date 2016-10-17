using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using API.Models.SafetySetting;
using Core.Domain.Business;
using Service.Interface.Business;
using Swashbuckle.Swagger.Annotations;
using Service.Models.YayYo;
using Service.Interface.YayYo;

namespace API.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1/safetysetting")]
    public class SafetySettingController : BaseApiController
    {
        #region fields

        private readonly ISafetySettingService _safetySettingService;
        private readonly ILogRideInformationService _rideInformationService;
        private readonly ISosGeolocationService _sosGeolocationService;
        private readonly ILogSosService _logSosService;
        private readonly IYayYoService _yayYoService;
        private readonly IContactService _contactService;
        #endregion

        #region ctor

        /// <summary>
        /// Safety Setting Controller
        /// </summary>
        /// <param name="safetySettingService"></param>
        /// <param name="rideInformationService"></param>
        /// <param name="sosGeolocationService"></param>
        /// <param name="yayYoService"></param>
        /// <param name="contactService"></param>
        /// <param name="logSosService"></param>
        public SafetySettingController(ISafetySettingService safetySettingService, ILogRideInformationService rideInformationService, ISosGeolocationService sosGeolocationService,
            IYayYoService yayYoService, IContactService contactService, ILogSosService logSosService)
        {
            _safetySettingService = safetySettingService;
            _rideInformationService = rideInformationService;
            _sosGeolocationService = sosGeolocationService;
            _yayYoService = yayYoService;
            _contactService = contactService;
            _logSosService = logSosService;
        }

        #endregion

        /// <summary>
        /// Insert/update safety settings     
        /// </summary>   
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.<br />
        /// <b>Description</b>: If update safety settings, please post old cancellation pin and old dupress pin
        /// </remarks>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerResponse(200, "Returns the result of creating safety settings")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> AddSafetySettings(CreateSafetySettingModel model)
        {
            //validate model
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                {
                    return BadRequest(ModelState[key].Errors[0].ErrorMessage);
                }
            }
            if (model.CancellationPin == model.DuressPin)
            {
                return BadRequest("Cancellation Pin and Duress Pin cannot be same");
            }
            //find safety settings
            var safetySetting = _safetySettingService.GetByYayYoId(model.YayYoId);
            try
            {
                if (safetySetting == null)
                {                    
                    //if not found, please add new safe settings
                    var newSafetySetting = new SafetySetting
                    {
                        YayYoId = model.YayYoId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        CancellationPin = model.CancellationPin,
                        DuressPin = model.DuressPin
                    };
                    await _safetySettingService.CreateAsync(newSafetySetting);
                    return Ok();
                }
                //if Update, require old cancellationPin and old duress pin
                if (string.IsNullOrEmpty(model.OldCancellationPin))
                {
                    return BadRequest("Old Cancellation Pin is required");
                }
                if (string.IsNullOrEmpty(model.OldDuressPin))
                {
                    return BadRequest("Old Duress Pin is required");
                }
                //if old pin != existing pin, return error
                if (model.OldCancellationPin != safetySetting.CancellationPin)
                {
                    return BadRequest("Current Cancellation Pin is not correct");
                }
                if (model.OldDuressPin != safetySetting.DuressPin)
                {
                    return BadRequest("Current Duress Pin is not correct");
                }
                safetySetting.CancellationPin = model.CancellationPin;
                safetySetting.DuressPin = model.DuressPin;
                await _safetySettingService.UpdateAsync(safetySetting);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);                
            }
        }

        /// <summary>
        /// Active SOS
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.
        /// </remarks>
        /// <returns></returns>
        [Route("sos/active")]
        [HttpPut]
        [SwaggerResponse(200, "Returns the result of active SOS")]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> ActiveSos(ActiveSosModel model)
        {
            //validate model
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                {
                    return BadRequest(ModelState[key].Errors[0].ErrorMessage);
                }
            }
            //find safety settings
            var safetySetting = _safetySettingService.GetByYayYoId(model.YayYoId);
            try
            {
                if (safetySetting == null)
                {
                    return BadRequest("User does not exist");
                }
                //update safety status
                safetySetting.Active = true;
                safetySetting.UpdatedOnUtc = DateTime.UtcNow;                
                await _safetySettingService.UpdateAsync(safetySetting);

                //create new SOS ID
                var logSos = new LogSos
                {
                    YayYoId = model.YayYoId,
                    SafetySettingId = safetySetting.Id                    
                };

                await _logSosService.CreateAsync(logSos);

                //get ride book info from YayYo
                var rideBookRequest = new GetLastestRideBookRequestModel { yayYo_id = model.YayYoId };
                var rideBookReponse = _yayYoService.GetRideBook(rideBookRequest);
                if (rideBookReponse == null)
                    return BadRequest("Error when get GetRideBook info");

                //add ride info to ride Information
                var rideBook = new LogRideInformation
                {
                    DriverName = rideBookReponse.driver_name,
                    CarMake = rideBookReponse.car_make,
                    CarModel = rideBookReponse.car_model,
                    CarColor = rideBookReponse.car_color,
                    CarLicense = rideBookReponse.car_license,
                    YayYoId = model.YayYoId,
                    LocationPickup = rideBookReponse.pickup_location,
                    TimeEta = rideBookReponse.time_eta,
                    TimePickup = rideBookReponse.time_pickup,
                    LocationDestination = rideBookReponse.destination_location,
                    LogSosId = logSos.Id
                };
                await _rideInformationService.CreateAsync(rideBook);

                //add SosGeolocation info
                var location = new LogSosGeolocation
                {
                    LogSosId = logSos.Id,
                    Location = model.GeoLocation
                };
                await _sosGeolocationService.CreateAsync(location);
                //send SMS
                var contacts = await _contactService.GetBySafetySettingIdAsync(safetySetting.Id);
                foreach (var contact in contacts )
                {
                    var contactInfo = new SmsRequestModel
                    {
                        FirstName = safetySetting.FirstName,
                        RiderPickupLocation = rideBook.LocationPickup,
                        DestinationAddress = rideBook.LocationDestination,
                        RideScheduledTime = rideBook.TimePickup ?? DateTime.MinValue,
                        GeolocationAddress = model.GeoLocation,
                        ContactPhoneNumber = contact.Phone
                    };
                    _yayYoService.SendSms(contactInfo);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deactive SOS
        /// </summary>
        /// <remarks>
        /// For native device, add "Authorization: Basic xxx" into Header of request.
        /// </remarks>
        /// <returns></returns>
        [Route("sos/deactive")]
        [HttpPut]
        [SwaggerResponse(200, "Returns the result of deactive SOS", typeof(DeactiveCancelSosResponseModel))]
        [SwaggerResponse(401, "Don't have permission")]
        [SwaggerResponse(500, "Internal Server Error")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IHttpActionResult> DeactiveSos(DeactiveSosModel model)
        {
            //validate model
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys.Where(key => ModelState[key].Errors.Count > 0))
                {
                    return BadRequest(ModelState[key].Errors[0].ErrorMessage);
                }
            }
            //find safety settings
            var safetySetting = _safetySettingService.GetByYayYoId(model.YayYoId);
            try
            {
                if (safetySetting == null)
                {
                    return BadRequest("User does not exist");
                }
                if (safetySetting.Active == false)
                {
                    return BadRequest("SOS is not active");
                }
                //check pin. If no pin is found, return error
                if (model.CancellationPin == safetySetting.CancellationPin)
                {
                    safetySetting.Active = false;
                    await _safetySettingService.UpdateAsync(safetySetting);
                    var response = new DeactiveCancelSosResponseModel
                    {
                        IsDuressMode = false
                    };
                    var logSos = await _logSosService.GetLastestLogSosAsync(model.YayYoId);
                    if (logSos == null)
                    {
                        return BadRequest("Log SOS does not exist");
                    }
                    logSos.UpdatedOnUtc = DateTime.UtcNow;
                    await _logSosService.UpdateAsync(logSos);
                    return Ok(response);
                }
                if (model.CancellationPin == safetySetting.DuressPin)
                {
                    var response = new DeactiveCancelSosResponseModel
                    {
                        IsDuressMode = true
                    };
                    var logSos = await _logSosService.GetLastestLogSosAsync(model.YayYoId);
                    if (logSos == null)
                    {
                        return BadRequest("Log SOS does not exist");
                    }
                    logSos.UpdatedOnUtc = DateTime.UtcNow;
                    await _logSosService.UpdateAsync(logSos);
                    return Ok(response);                        
                }
                return BadRequest("Incorrect Pin");                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
