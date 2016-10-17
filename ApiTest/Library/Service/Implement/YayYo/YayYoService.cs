using System;
using System.Collections.Generic;
using System.Configuration;
using Common.Logs;
using RestSharp;
using Service.Interface.YayYo;
using Service.Models.YayYo;
using Service.Interface.Twilio;

namespace Service.Implement.YayYo
{
    public class YayYoService: BaseServiceWithLogging, IYayYoService
    {
        #region fields
        private static readonly string YayYoHost = ConfigurationManager.AppSettings["YayYo-Host"];
        private const string GetRideBookUrl = "api/RideBook/GetRideBookInfo";

        private readonly ITwilioService _twilioService;
        #endregion

        #region ctor
        public YayYoService(INoisLoggingService noisLoggingService, ITwilioService twilioService) : base(noisLoggingService)
        {
            _twilioService = twilioService;
        }
        #endregion
        public GetLastestRideBookResponseModel GetRideBook(GetLastestRideBookRequestModel model)
        {
            try
            {
                var response = Post(model, GetRideBookUrl);
                //add fake object                
                return new GetLastestRideBookResponseModel
                {
                    driver_name = "Driver name",
                    car_make = "Ford",
                    car_model = "F-16",
                    car_color = "Silver",
                    car_license = "GT117",
                    time_eta = 30,
                    pickup_location = "Home",
                    time_pickup = DateTime.UtcNow,
                    destination_location = "Work"
                };
                //if (string.IsNullOrEmpty(response.Content))
                //{
                //    return null;
                //}
                //return JsonConvert.DeserializeObject<GetLastestRideBookResponseModel>(response.Content);
            }
            catch (Exception ex)
            {
                LogError("There is error while call company signup api: " + ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <returns></returns>
        public bool SendSms(SmsRequestModel contactInfo)
        {
            try
            {
                var smsBody = string.Format(
                        "Emergency contact first name: {0}, {0} has activated a safety feature through our YayYo ride-sharing application. " +
                        "{0} was picked up at: {1} from: {2}. {0} was to be dropped off at: {3}.",
                        contactInfo.FirstName, contactInfo.RideScheduledTime, contactInfo.RiderPickupLocation, contactInfo.DestinationAddress);
                if (!string.IsNullOrEmpty(contactInfo.GeolocationAddress))
                {
                    var geolocationAddress = string.Format(" {0} last known location is: {1}", contactInfo.FirstName, contactInfo.GeolocationAddress);
                    smsBody += geolocationAddress;
                }
                var fromPhone = ConfigurationManager.AppSettings["TwilioFromPhone"];                
                var messageResponse = _twilioService.SendMessage(fromPhone, contactInfo.ContactPhoneNumber, smsBody);
                if (messageResponse == null || messageResponse.ErrorCode != 0)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region utils

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="model"></param>
        /// <param name="absolutePath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private IRestResponse Post(object model, string absolutePath, string token = null)
        {
            var client = new RestClient(YayYoHost);
            var request = new RestRequest(absolutePath, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            //add body
            if (model != null)
            {
                request.AddBody(model);
            }
            //add header
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("x-access-token", token);
            }
            var response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="model"></param>
        /// <param name="absolutePath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private IRestResponse Put(object model, string absolutePath, string token = null)
        {
            var client = new RestClient(YayYoHost);
            var request = new RestRequest(absolutePath, Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };
            //add body
            if (model != null)
            {
                request.AddBody(model);
            }
            //add header
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("x-access-token", token);
            }
            var response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="absolutePath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private IRestResponse Get(Dictionary<string, string> parameters, string absolutePath, string token = null)
        {
            var client = new RestClient(YayYoHost);
            var request = new RestRequest(absolutePath, Method.GET);
            //add header
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("x-access-token", token);
            }
            //add params
            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
            var response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="absolutePath"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private IRestResponse Delete(Dictionary<string, string> parameters, string absolutePath, string token = null)
        {
            var client = new RestClient(YayYoHost);
            var request = new RestRequest(absolutePath, Method.DELETE);
            //add header
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("x-access-token", token);
            }
            //add params
            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
            var response = client.Execute(request);
            return response;
        }
        #endregion
    }
}
