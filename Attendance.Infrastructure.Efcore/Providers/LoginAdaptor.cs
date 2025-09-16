using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class LoginAdaptor : ILoginAdaptor
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private readonly GlobalClass _globalClass;

        public LoginAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
            _globalClass = globalClass;
        }
        public async Task<string> PostApiDataAsync(Employee employeeModel)
        {
            try
            {
                var baseUrl = apiCredential.url + "Login";

                var user = JsonConvert.SerializeObject(employeeModel);
                var requestContent = new StringContent(user, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(baseUrl, requestContent);
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null)
                {
                    var responseToken = JsonConvert.DeserializeObject<ResponseToken>(responseModel?.Data.ToString()!);
                    return responseToken.Token;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
        public class ResponseToken
        {
            public string? Token { get; set; }
        }
    }
}
