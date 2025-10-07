using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Attendance.Infrastructure.Provider
{
    public class UserMenuMappingAdaptor : IUserMenuMappingAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        public UserMenuMappingAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<IEnumerable<UserMenuMappingDto>> GetUserMenuById(int id)
        {
            try
            {
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
                var response = await _httpClient.GetAsync(apiCredential.url + "UserMenuMapping/GetUserMenuMappingById/" + id);
                    var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null)
                {
                    var menulist = JsonConvert.DeserializeObject<IEnumerable<UserMenuMappingDto>>(Convert.ToString(responseModel.Data!));
                    return menulist;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserMenuMappingDto> GetById(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "UserMenuMapping/GetUserMenuMappingById/" + id);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<UserMenuMappingDto>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        public async Task<IEnumerable<UserMenuMappingDto>> GetAll()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "UserMenuMapping/GetAllUserMenuMapping");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<UserMenuMappingDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return new List<UserMenuMappingDto>();
        }
        public async Task<string> AddUserMenuMapping(UserMenuMappingDto userMenuMapping)
        {
            try
            {
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
                var content = new StringContent(JsonConvert.SerializeObject(userMenuMapping), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiCredential.url + "UserMenuMapping/AddUserMenuMapping", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null && response.IsSuccessStatusCode)
                {
                    return "UserMenuMapping Added succesfully";
                }
                else
                {
                    return "UserMenuMapping not Added";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> UpdateMenuMapping(UserMenuMappingDto userMenuMapping, int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var content = new StringContent(JsonConvert.SerializeObject(userMenuMapping), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiCredential.url + "UserMenuMapping/UpdateUserMenuMapping/" + id, content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null && response.IsSuccessStatusCode)
            {
                return "UserMenuMapping Updated successfully";
            }
            else
            {
                return "UserMenuMapping Updated Failed";
            }
        }
        public async Task<int> DeleteUserMenuMapping(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.DeleteAsync(apiCredential.url + "UserMenuMapping/DeleteUserMenuMapping/" + id);
            if (response.IsSuccessStatusCode)
            {
                return id;
            }
            return 0;
        }
        public async Task<List<UserDto>> GetAllUserAsync()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);

            var response = await _httpClient.GetAsync(apiCredential.url + "User/GetAllUsers'");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

            if (responseModel != null && responseModel.Data != null)
            {
                var details = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(responseModel.Data));
                return details;
            }
            return new List<UserDto>();
        }
    }
}
