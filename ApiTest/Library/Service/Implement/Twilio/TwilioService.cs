using System.Configuration;
using Service.Interface.Twilio;
using Twilio;

namespace Service.Implement.Twilio
{
    public class TwilioService: ITwilioService
    {
        private readonly TwilioRestClient _client;
        private static readonly string TwilioAccountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
        private static readonly string TwilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];

        public TwilioService()
        {
            _client = new TwilioRestClient(TwilioAccountSid, TwilioAuthToken);
        }

        public TwilioService(TwilioRestClient client)
        {
            _client = client;
        }

        public Message SendMessage(string from, string to, string body)
        {
           return _client.SendMessage(from, to, body);
        }
    }
}
