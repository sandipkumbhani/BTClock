using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Attendance.Infrastructure.Provider
{
    public class MenuMasterAdaptor : IMenuMasterAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        public MenuMasterAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<IEnumerable<menuMasterDto>> GetAllMenuMasters()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "MenuMaster/GetAllMenuMasters");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<menuMasterDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }

        public async Task<IEnumerable<menuMasterDto>> GetMenuMasterById(int id)
        {
            try
            {
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
                var response = await _httpClient.GetAsync(apiCredential.url + "/MenuMaster/GetMenuMasterById/" + id);
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null)
                {
                    var menulist = JsonConvert.DeserializeObject<List<menuMasterDto>>(Convert.ToString(responseModel.Data!));
                    return menulist;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<menuMasterDto> GetById(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "MenuMaster/GetMenuMasterById/" + id);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<menuMasterDto>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }

        public async Task<string> AddMenuMaster(menuMasterDto menu)
        {
            try
            {
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
                var content = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiCredential.url + "MenuMaster/AddMenuMaster", content);
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null)
                {
                    var result = responseModel.StatusCode;
                    if (result == 200)
                    {
                        return "add menu succesfully";
                    }
                    else
                    {
                        return "menu not add";
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }

        public async Task<string> UpdateMenuMaster(menuMasterDto menu, int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var content = new StringContent(JsonConvert.SerializeObject(menu), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiCredential.url + "MenuMaster/UpdateMenuMaster/" + id, content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
           if (responseModel != null && response.IsSuccessStatusCode)
            {
                return "Menu Updated successfully";
            }
            else
            {
                return "Menu Updated Failed";
            }
        }

        public async Task<int> DeleteMenuMaster(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.DeleteAsync(apiCredential.url + "MenuMaster/DeleteMenuMaster/" + id);
            if (response.IsSuccessStatusCode)
            {
                return id;
            }
            return 0;
        }
    }
}

