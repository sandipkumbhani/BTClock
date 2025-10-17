using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class MenuItemAdaptor : IMenuItemAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly APICredential _apiCredential;

        public MenuItemAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _apiCredential = new APICredential(configuration);
        }

        private HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            return client;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}MenuItem/GetAllMenuItems");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel?.Data != null)
                return JsonConvert.DeserializeObject<List<MenuItemDto>>(Convert.ToString(responseModel.Data));

            return new List<MenuItemDto>();
        }

        public async Task<MenuItemDto> GetMenuItemByIdAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}MenuItem/GetMenuItemById/{id}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel?.Data != null)
                return JsonConvert.DeserializeObject<MenuItemDto>(Convert.ToString(responseModel.Data));

            return null;
        }

        public async Task<IEnumerable<MenuItemDto>> GetMenuItemsByMenuIdAsync(int menuId)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}MenuItem/GetMenuItemsByMenuId/{menuId}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel?.Data != null)
                return JsonConvert.DeserializeObject<List<MenuItemDto>>(Convert.ToString(responseModel.Data));

            return new List<MenuItemDto>();
        }

        public async Task<string> AddMenuItemAsync(MenuItemDto menuItemDto)
        {
            var client = GetHttpClient();
            var jsonContent = JsonConvert.SerializeObject(menuItemDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiCredential.url}MenuItem/AddMenuItem", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            return responseModel?.Message ?? "Error adding menu item";
        }

        public async Task<string> UpdateMenuItemAsync(MenuItemDto menuItemDto, int id)
        {
            var client = GetHttpClient();
            var jsonContent = JsonConvert.SerializeObject(menuItemDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_apiCredential.url}MenuItem/UpdateMenuItem/{id}", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            return responseModel?.Message ?? "Error updating menu item";
        }

        public async Task<int> DeleteMenuItemAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"{_apiCredential.url}MenuItem/DeleteMenuItem/{id}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            return responseModel?.StatusCode ?? 0;
        }
        public async Task<IEnumerable<MenuItemDto>> GetAccessibleMenuItemsAsync(int userId)
        {
            try
            {
                var client = GetHttpClient();
                var response = await client.GetAsync($"{_apiCredential.url}MenuItem/GetAccessibleMenuItems/{userId}");
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

                if (responseModel?.Data != null)
                    return JsonConvert.DeserializeObject<List<MenuItemDto>>(responseModel.Data.ToString());

                return new List<MenuItemDto>();
            }
            catch
            {
                return new List<MenuItemDto>();
            }
        }
    }
}
