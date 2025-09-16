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
	public class DepartmentAdaptor : IDepartmentAdaptor
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public DepartmentAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
		{
			var httpClient = _httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Department/GetDepartmentById/" + departmentId;
			var response = await httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var employee = JsonConvert.DeserializeObject<DepartmentDto>(responseModel.Data.ToString());
				return employee;
			}
			return null;
		}
		public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
		{
			var httpClient = _httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Department/GetAllDepartment";
			var response = await httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				var employee = JsonConvert.DeserializeObject<List<DepartmentDto>>(responseModel.Data.ToString());
				return employee;
			}
			return null;
		}
		public async Task<string> AddDepartmentAsync(DepartmentDto departmentDto)
		 {
			try
			{

				var httpclient = new HttpClient();
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
				var baseUrl = _apiCredential.url + "Department/AddDepartment";
				var patient = JsonConvert.SerializeObject(departmentDto);
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
			catch (Exception ex)
			{
				return null;
			}
		}
		public async Task<string> UpdateDepartmentAsync(DepartmentDto departmentDto, int departmentId)
		{
			var httpClient = _httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Department/UpdateDepartment/" + departmentId;
			var response = await httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				var employee = JsonConvert.DeserializeObject<string>(responseModel.Data.ToString());
				return employee;
			}
			return null;
		}
		public async Task<int> DeleteDepartmentAsync(int departmentId)
		{
			var httpClient = _httpClient;
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Department/DeleteDepartment/" + departmentId;
			var response = await httpClient.GetAsync(baseUrl);

			if (response.IsSuccessStatusCode)
			{
				return departmentId;
			}
			return 0;
		}
	}
}
