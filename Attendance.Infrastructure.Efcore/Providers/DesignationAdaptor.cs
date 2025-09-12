using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.UI.Domain.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
	public class DesignationAdaptor : IDesignationAdaptor
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public DesignationAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<List<DesignationDto>> GetAllDesignationAsync(DesignationDto designationDto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Designation/GetAllDesignation";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var designation = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DesignationDto>>(responseModel.Data.ToString());
				return designation;
			}
			return null;
		}
		public async Task<DesignationDto> GetDesignationByIdAsync(int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Designation/GetDesignationById/" + Id;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var designation = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignationDto>(responseModel.Data.ToString());
				return designation;
			}
			return null;
		}
		public async Task<string> AddDesignationAsync(DesignationDto designationdto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Designation/AddDesignation";
			var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(designationdto);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, content);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();
			}
			return null;
		}
		public async Task<string> UpdateDesignationAsync(DesignationDto designationdto, int designationId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Designation/UpdateDesignation/" + designationId;
			var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(designationdto);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(baseUrl, content);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();
			}
			return null;
		}
		public async Task<int> DeleteDesignationAsync(int designationId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Designation/DeleteDesignation/" + designationId;
			var response = await _httpClient.DeleteAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				return designationId;	
			}
			return 0;
		}
	}
}
