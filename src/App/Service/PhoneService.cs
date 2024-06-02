using System.Net;
using System.Net.Http.Headers;

using stela_api.src.App.IService;
using stela_api.src.Domain.Entities.Config;

namespace stela_api.src.App.Service
{
    public class PhoneService : IPhoneService
    {
        private readonly PhoneServiceSettings _serviceSettings;
        private readonly string _baseUrl = "https://api.exolve.ru/messaging/v1/SendSMS";

        private readonly ILogger<PhoneService> _logger;
        public PhoneService(
            PhoneServiceSettings settings,
            ILogger<PhoneService> logger)
        {
            _serviceSettings = settings;
            _logger = logger;
        }

        public async Task<bool> SendMessage(string destinationPhoneNumber, string message)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", _serviceSettings.ApiKey);

                var requestBody = new
                {
                    number = _serviceSettings.SenderPhone,
                    destination = destinationPhoneNumber,
                    text = message
                };

                var response = await client.PostAsJsonAsync(_baseUrl, requestBody);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}