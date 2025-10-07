using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class MenuItemAdaptor : IMenuItemAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public MenuItemAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }
        public async Task<IEnumerable<MenuItemDto>> GetAllAsync()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "MenuItem/GetAllMenuItems");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MenuItemDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        public async Task<MenuItemDto> GetByIdAsync(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "MenuItem/GetMenuItemById/" + id);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = Newtonsoft.Json.JsonConvert.DeserializeObject<MenuItemDto>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        public async Task<IEnumerable<MenuItemDto>> GetByMenuIdAsync(int menuId)
        {
            try
            {
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
                var response = await _httpClient.GetAsync(apiCredential.url + "MenuItem/GetMenuItemsByMenuId/" + menuId);
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null)
                {
                    var menulist = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<MenuItemDto>>(Convert.ToString(responseModel.Data!));
                    return menulist;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> AddMenuItemAsync(MenuItemDto menuItem)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(menuItem);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiCredential.url + "MenuItem/AddMenuItem", contentString);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                return Convert.ToString(responseModel.Message!);
            }
            return null;
        }
        public async Task<string> UpdateMenuItemAsync(MenuItemDto menuItem, int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(menuItem);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiCredential.url + "MenuItem/UpdateMenuItem/" + id, contentString);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                return Convert.ToString(responseModel.Message!);
            }
            return null;
        }

        public async Task<int> DeleteMenuItemAsync(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.DeleteAsync(apiCredential.url + "MenuItem/DeleteMenuItem/" + id);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                return Convert.ToInt32(responseModel.StatusCode);
            }
            return 0;
        }
    }
}
