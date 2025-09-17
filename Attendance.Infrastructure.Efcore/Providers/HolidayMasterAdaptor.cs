using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Attendance.Infrastructure.Efcore.Providers
{
   public  class HolidayMasterAdaptor : IHolidayMasterAdaptor
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public HolidayMasterAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<List<HolidayMasterDto>> GetAllHolidayMasterAsync()
		{
			var httpClient = _httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "HolidayMaster/GetAllHolidayMasters";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var holidayMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HolidayMasterDto>>(responseModel.Data.ToString());
				return holidayMaster;
			}
			return null;
		}
		public async Task<HolidayMasterDto> GetHolidayMasterByIdAsync(int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "HolidayMaster/GetHolidayMasterById/" + Id;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var holidayMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<HolidayMasterDto>(responseModel.Data.ToString());
				return holidayMaster;
			}
			return null;
		}
		public async Task<string> AddHolidayMasterAsync(HolidayMasterDto holidayMasterdto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "HolidayMaster/AddHolidayMaster";
			var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(holidayMasterdto);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, content);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Message;
			}
			return null;
		}
		public async Task<string> UpdateHolidayMasterAsync(HolidayMasterDto holidayMasterdto, int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "HolidayMaster/UpdateHolidayMaster/" + Id;
			var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(holidayMasterdto);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(baseUrl, content);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Message;
			}
			return null;
		}
		public async Task<int> DeleteHolidayMasterAsync(int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "HolidayMaster/DeleteHolidayMaster/" + Id;
			var response = await _httpClient.DeleteAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				return Id;
			}
			return 0;
		}
	}
}
