using Microsoft.Extensions.Configuration;
using Twilio.Clients;
using Twilio.Http;
using System.Threading.Tasks;
using MiniFB.Infrastructure.Models;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MiniFB.BAL.Twilio
{
    public class CustomTwilioClient : ITwilioRestClient
    {
        private readonly ITwilioRestClient innerClient;

        public CustomTwilioClient(IConfiguration config, System.Net.Http.HttpClient httpClient)
        {
            
            httpClient.DefaultRequestHeaders.Add("X-Custom-Header", "CustomTwilioRestClient-Demo");

            innerClient = new TwilioRestClient(
                config["Twilio:AccountSid"],
                config["Twilio:AuthToken"],
                httpClient: new SystemNetHttpClient(httpClient));
        }

        public Response Request(Request request) => innerClient.Request(request);
        public Task<Response> RequestAsync(Request request) => innerClient.RequestAsync(request);
        public string AccountSid => innerClient.AccountSid;
        public string Region => innerClient.Region;
        HttpClient ITwilioRestClient.HttpClient => innerClient.HttpClient;
    }
}
