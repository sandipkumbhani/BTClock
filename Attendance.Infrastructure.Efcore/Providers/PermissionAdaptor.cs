using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
   public  class PermissionAdaptor:IPermissionAdaptor
    {
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public PermissionAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<List<PermissionDto>> GetAllPermissionsAsync()
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Permission/GetAllPermissions";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var permission = JsonConvert.DeserializeObject<List<PermissionDto>>(responseModel.Data.ToString());

				return permission;
			}
			return null;
		}
		public async Task<PermissionDto> GetPermissionByIdAsync(int permissionId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Permission/GetPermissionById/" + permissionId;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var permission = JsonConvert.DeserializeObject<PermissionDto>(responseModel.Data.ToString());

				return permission;
			}
			return null;
		}
		public async Task<string> AddPermissionAsync(PermissionDto permissionDto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Permission/AddPermission";
			var patient = JsonConvert.SerializeObject(permissionDto);
			var requestContent = new StringContent(patient, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, requestContent);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();
				
			}
			return null;
		}
		public async Task<string> UpdatePermissionAsync(PermissionDto permissionDto, int permissionId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Permission/UpdatePermission/" + permissionId;
			var patient = JsonConvert.SerializeObject(permissionDto);
			var requestContent = new StringContent(patient, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(baseUrl, requestContent);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();
			}
			return null;
		}
		public async Task<int> DeletePermissionAsync(int permissionId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Permission/DeletePermission/" + permissionId;
			var response = await _httpClient.DeleteAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				return permissionId;
			}
			return 0;
		}
	}
}
