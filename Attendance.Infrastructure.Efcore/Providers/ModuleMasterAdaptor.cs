using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
	public class ModuleMasterAdaptor : IModuleMasterAdaptor
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public ModuleMasterAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<List<ModuleMasterDto>> GetAllModuleMasterAsync()
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "ModuleMaster/GetAllModuleMaster";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var moduleMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuleMasterDto>>(responseModel.Data.ToString());
				return moduleMaster;
			}
			return null;
		}
		public async Task<ModuleMasterDto> GetModuleMasterByIdAsync(int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "ModuleMaster/GetModuleById/" + Id;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var moduleMaster = Newtonsoft.Json.JsonConvert.DeserializeObject<ModuleMasterDto>(responseModel.Data.ToString());
				return moduleMaster;
			}
			return null;
		}
		public async Task<string> AddModuleMasterAsync(ModuleMasterDto moduleMasterDto)
		{
			try
			{

				var httpclient = new HttpClient();
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
				var baseUrl = _apiCredential.url + "ModuleMaster/AddModule";
				var patient = JsonConvert.SerializeObject(moduleMasterDto);
				var requestContent = new StringContent(patient, Encoding.UTF8, "application/json");
				var response = await _httpClient.PostAsync(baseUrl, requestContent);
				var responseData = await response.Content.ReadAsStringAsync();
				var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
				if (response.IsSuccessStatusCode)
				{
					return responseModel.Data.ToString();
				}
				return null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public async Task<string> UpdateModuleMasterAsync(ModuleMasterDto moduleMasterDto, int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "ModuleMaster/UpdateModule/" + Id;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Data.ToString();
			}
			return null;
		}
		public async Task<int> DeleteModuleMasterAsync(int Id)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "ModuleMaster/DeleteModule/" + Id;
			var response = await _httpClient.GetAsync(baseUrl);

			if (response.IsSuccessStatusCode)
			{
				return Id;
			}
			return 0;
		}
	}
}
