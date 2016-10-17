using Service.Models.YayYo;

namespace Service.Interface.YayYo
{
    public interface IYayYoService
    {
        /// <summary>
        /// Get ridebook information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        GetLastestRideBookResponseModel GetRideBook(GetLastestRideBookRequestModel model);

        bool SendSms(SmsRequestModel contactInfo);
    }
}
