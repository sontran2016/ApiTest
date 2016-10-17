using Twilio;

namespace Service.Interface.Twilio
{
    public interface ITwilioService
    {
        /// <summary>
        /// Send Message. Fake function. Always return true
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Message SendMessage(string from, string to, string body);
    }
}
