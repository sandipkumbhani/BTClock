using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
   public class LeaveMasterAdaptor:ILeaveMasterAdaptor
    {
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public LeaveMasterAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<List<LeaveMasterDto>> GetAllLeaveMastersAsync()
		{
			 var httpClient = _httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "LeaveMaster/GetAllLeaveMasters";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var leaveMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LeaveMasterDto>>(responseModel.Data.ToString());
				return leaveMaster;
			}
			return null;
		}
		public async Task<LeaveMasterDto> GetLeaveMasterByIdAsync(int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "LeaveMaster/GetLeaveMasterById/" + Id;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var leaveMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<LeaveMasterDto>(responseModel.Data.ToString());
				return leaveMaster;
			}
			return null;
		}
		public async Task<string> AddLeaveMasterAsync(LeaveMasterDto leaveMasterdto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "LeaveMaster/AddLeaveMaster";
			var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(leaveMasterdto);
			var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, contentData);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();

			}
			return null;
		}
		public async Task<string> UpdateLeaveMasterAsync(LeaveMasterDto leaveMasterdto, int leavemsterId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "LeaveMaster/UpdateLeaveMaster/" + leavemsterId;
			var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(leaveMasterdto);
			var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(baseUrl, contentData);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();

			}
			return null;
		}
		public async Task<int> DeleteLeaveMasterAsync(int leavemsterId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "LeaveMaster/DeleteLeaveMaster/" + leavemsterId;
			var response = await _httpClient.DeleteAsync(baseUrl);
			
			if (response.IsSuccessStatusCode)
			{
				return leavemsterId;
			}
			return 0;
		}
	}
}
