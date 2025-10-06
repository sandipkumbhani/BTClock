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
	public class UserAdaptor : IUserAdaptor
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly APICredential _apiCredential;
		private readonly GlobalClass _globalClass;
		public UserAdaptor(HttpClient httpClient, IConfiguration configuration, GlobalClass globalClass)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_apiCredential = new APICredential(_configuration);
			_globalClass = globalClass;
		}
		public async Task<UserDto> GetUserByIdAsync(int userId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "User/GetUserById/" + userId;
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var user = JsonConvert.DeserializeObject<UserDto>(responseModel.Data.ToString());
				return user;
			}
			return null;
		}
		public async Task<List<UserDto>> GetAllUserAsync()
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "User/GetAllUsers";
			var response = await _httpClient.GetAsync(baseUrl);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				// Deserialize response data to PatientDto
				var user = JsonConvert.DeserializeObject<List<UserDto>>(responseModel.Data.ToString());
				return user;
			}
			return null;
		}
		public async Task<string> AddUserAsync(UserDto userDto)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "User/AddUser";
			var jsonContent = JsonConvert.SerializeObject(userDto);
			var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(baseUrl, contentString);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Message.ToString();
			}
			return responseModel.Message;
		}
		public async Task<string> UpdateUserAsync(UserDto userDto, int userId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "User/UpdateUser/" + userId;
			var jsonContent = JsonConvert.SerializeObject(userDto);
			var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(baseUrl, contentString);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
			{
				return responseModel.Message.ToString();
			}
			return responseModel.Message;
		}
		public async Task<int> DeleteUserAsync(int userId)
		{
			var httpclient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var baseUrl = _apiCredential.url + "User/DeleteUser/" + userId;
			var response = await _httpClient.DeleteAsync(baseUrl);
			if (response.IsSuccessStatusCode)
			{
				return userId;
			}
			return 0;
		}
	}
}
