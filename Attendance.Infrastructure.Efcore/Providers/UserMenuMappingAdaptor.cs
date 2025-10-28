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
        private readonly APICredential _apiCredential;

        public UserMenuMappingAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            _apiCredential = new APICredential(configuration);
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            return client;
        }

        public async Task<IEnumerable<UserMenuMappingDto>> GetAllUserMenuMappingsAsync()
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}UserMenuMapping/GetAllUserMenuMapping");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

            return responseModel != null
                ? JsonConvert.DeserializeObject<List<UserMenuMappingDto>>(Convert.ToString(responseModel.Data!))!
                : new List<UserMenuMappingDto>();
        }

        public async Task<IEnumerable<UserMenuMappingDto>> GetUserMenuMappingsByUserIdAsync(int userId)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}UserMenuMapping/GetUserMenuMappingByUserId/{userId}");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

            return responseModel != null
                ? JsonConvert.DeserializeObject<List<UserMenuMappingDto>>(Convert.ToString(responseModel.Data!))!
                : new List<UserMenuMappingDto>();
        }

        public async Task<UserMenuMappingDto> GetUserMenuMappingByIdAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}UserMenuMapping/GetUserMenuMappingById/{id}");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

            return responseModel != null
                ? JsonConvert.DeserializeObject<UserMenuMappingDto>(Convert.ToString(responseModel.Data!))!
                : null!;
        }

        public async Task<string> AddUserMenuMappingAsync(UserMenuMappingDto userMenuMappingDto)
        {
            try
            {
                using var client = CreateHttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(userMenuMappingDto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_apiCredential.url}UserMenuMapping/AddUserMenuMapping", content);

                return response.IsSuccessStatusCode ? "User menu mapping added successfully" : "Failed to add user menu mapping";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> UpdateUserMenuMappingAsync(int id, UserMenuMappingDto userMenuMappingDto)
        {
            using var client = CreateHttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(userMenuMappingDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_apiCredential.url}UserMenuMapping/UpdateUserMenuMapping/{id}", content);

            return response.IsSuccessStatusCode ? "User menu mapping updated successfully" : "Failed to update user menu mapping";
        }

        public async Task<int> DeleteUserMenuMappingAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.DeleteAsync($"{_apiCredential.url}UserMenuMapping/DeleteUserMenuMapping/{id}");
            return response.IsSuccessStatusCode ? id : 0;
        }

        public async Task<string> UpdateUserMenuMappingsForUserAsync(int userId, List<int> menuIds)
        {
            try
            {
                using var client = CreateHttpClient();
                var content = new StringContent(JsonConvert.SerializeObject(menuIds), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{_apiCredential.url}UserMenuMapping/UpdateUserMenuMappingsForUser/{userId}", content);

                return response.IsSuccessStatusCode
                    ? "User menu mappings updated successfully"
                    : "Failed to update user menu mappings";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<IEnumerable<MenuItemDto>> GetAccessibleMenusByUserIdAsync(int userId)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}MenuItem/GetAccessibleMenus/{userId}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

            if (responseModel?.Data != null)
            {
                return JsonConvert.DeserializeObject<List<MenuItemDto>>(Convert.ToString(responseModel.Data));
            }

            return new List<MenuItemDto>();
        }
    }
}
