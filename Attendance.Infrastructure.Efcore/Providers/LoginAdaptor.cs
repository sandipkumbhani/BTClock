using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class LoginAdaptor : ILoginAdaptor
    {
        private readonly HttpClient _httpClient;
        private readonly APICredential _apiCredential;
        private readonly GlobalClass _globalClass;

        public LoginAdaptor(IHttpClientFactory httpClientFactory, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }

        public async Task<LoginResponseData?> LoginAsync(User model)
        {
            try
            {
                var baseUrl = _apiCredential.url + "login";
                var userJson = JsonConvert.SerializeObject(model);

                var content = new StringContent(userJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(baseUrl, content);
                var responseData = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Raw API response: " + responseData);

                var loginResponse = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

                if (loginResponse?.Data != null)
                {
                    var loginData = JsonConvert.DeserializeObject<LoginResponseData>(Convert.ToString(loginResponse.Data));
                    return loginData;
                }

                Console.WriteLine("Login failed: Data was null or invalid");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoginAdaptor Error: {ex.Message}");
                return null;
            }
        }


    }
}
