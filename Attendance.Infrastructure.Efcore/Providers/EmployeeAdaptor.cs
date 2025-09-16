using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.Domain.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Attendance.Infrastructure.Efcore.Providers
{
	public class EmployeeAdaptor : IEmployeeAdaptor
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public EmployeeAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Employee/GetEmployeeById/" + employeeId;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
					var employee = JsonConvert.DeserializeObject<EmployeeDto>(responseModel.Data.ToString());
				return employee;
			}
			return null;
		}
		public async Task<List<EmployeeDto>>GetAllEmployeeAsync(EmployeeDto employeedto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Employee/GetAllEmployees";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);

			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var employee = JsonConvert.DeserializeObject<List<EmployeeDto>>(responseModel.Data.ToString());
				return employee;
			}
			return null;
		}
		public async Task<string> AddEmployeeAsync(EmployeeDto employeedto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Employee/AddEmployee";
			var patient = JsonConvert.SerializeObject(employeedto);
			var requestContent = new StringContent(patient, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, requestContent);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				return responseModel.Data.ToString();
			}
			return null;
		}
		public async Task<string> UpdateEmployeeAsync(EmployeeDto employeedto, int employeeId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Employee/UpdateEmployee/" + employeeId;
			var patient = JsonConvert.SerializeObject(employeedto);
			var requestContent = new StringContent(patient, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(baseUrl, requestContent);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				return responseModel.Data.ToString();
			}
			return null;
		}
		public async Task<int> DeleteEmployeeAsync(int employeeId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "Employee/DeleteEmployee/" + employeeId;
			var response = await _httpClient.DeleteAsync(baseUrl);
			
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				return employeeId;
			}
			return 0;
		}
	}
}
