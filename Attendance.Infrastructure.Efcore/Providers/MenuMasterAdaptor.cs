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
    public class MenuMasterAdaptor : IMenuMasterAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly APICredential _apiCredential;

        public MenuMasterAdaptor(GlobalClass globalClass, IConfiguration configuration)
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

        public async Task<IEnumerable<MenuMasterDto>> GetAllMenuMastersAsync()
        {
            var client = GetHttpClient();
            var response = await client.GetAsync(_apiCredential.url+"MenuMaster/GetAllMenuMaster");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<MenuMasterDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }

        public async Task<MenuMasterDto> GetByIdAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}MenuMaster/GetMenuMasterById/{id}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel?.Data != null)
            {
                return JsonConvert.DeserializeObject<MenuMasterDto>(Convert.ToString(responseModel.Data));
            }
            return null;
        }

        public async Task<IEnumerable<MenuMasterDto>> GetMenuMasterByIdAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.GetAsync($"{_apiCredential.url}MenuMaster/GetMenuMasterById/{id}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel?.Data != null)
            {
                return JsonConvert.DeserializeObject<List<MenuMasterDto>>(Convert.ToString(responseModel.Data));
            }
            return new List<MenuMasterDto>();
        }

        public async Task<string> AddMenuMasterAsync(MenuMasterDto menuMasterDto)
        {
            var client = GetHttpClient();
            var jsonContent = JsonConvert.SerializeObject(menuMasterDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_apiCredential.url}MenuMaster/AddMenuMaster", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            return responseModel?.Message ?? "Error adding menu master";
        }

        public async Task<string> UpdateMenuMasterAsync(MenuMasterDto menuMasterDto, int id)
        {
            var client = GetHttpClient();
            var jsonContent = JsonConvert.SerializeObject(menuMasterDto);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_apiCredential.url}MenuMaster/UpdateMenuMaster/{id}", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            return responseModel?.Message ?? "Error updating menu master";
        }

        public async Task<int> DeleteMenuMasterAsync(int id)
        {
            var client = GetHttpClient();
            var response = await client.DeleteAsync($"{_apiCredential.url}MenuMaster/DeleteMenuMaster/{id}");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            return responseModel?.StatusCode ?? 0;
        }
    }
}
