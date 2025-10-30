using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Attendance.Infrastructure.Efcore.Providers
{
	public class LeaveTransactionAdaptor : ILeaveTransactionAdaptor
	{
		private readonly GlobalClass _globalClass;
		private readonly IConfiguration _configuration;
		private APICredential apiCredential;

		public LeaveTransactionAdaptor(IConfiguration configuration, GlobalClass globalClass)
		{
			_configuration = configuration;
			_globalClass = globalClass;
			apiCredential = new APICredential(configuration);

		}
		public async Task<List<LeaveTransactionDto>> GetAllLeaveTransactions()
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var response = await _httpClient.GetAsync(apiCredential.url + "LeaveTransaction/GetAllLeaveTransactions");
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (responseModel != null)
			{
				var details = JsonConvert.DeserializeObject<List<LeaveTransactionDto>>(Convert.ToString(responseModel.Data!));
				return details;
			}
			return null;
		}
		public async Task<IEnumerable<LeaveTransactionDto>> GetLeaveTransactionsByUserId(int userId)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var response = await _httpClient.GetAsync(apiCredential.url + "LeaveTransaction/GetLeaveTransactionsByUserId/" + userId);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (responseModel != null)
			{
				var details = JsonConvert.DeserializeObject<List<LeaveTransactionDto>>(Convert.ToString(responseModel.Data!));
				return details;
			}
			return null;
		}
		public async Task<LeaveTransactionDto> GetLeaveTransactionById(int id)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var response = await _httpClient.GetAsync(apiCredential.url + "LeaveTransaction/GetLeaveTransactionById/" + id);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (responseModel != null)
			{
				var details = JsonConvert.DeserializeObject<LeaveTransactionDto>(Convert.ToString(responseModel.Data!));
				return details;
			}
			return null;
		}
		public async Task<string> AddLeaveTransaction(LeaveTransactionDto leaveTransaction)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var content = new StringContent(JsonConvert.SerializeObject(leaveTransaction), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(apiCredential.url + "LeaveTransaction/AddLeaveTransaction", content);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (response.IsSuccessStatusCode)
				return "Leave Added Successfully";
			else
				return $"Failed: {responseModel?.ErrorMessage ?? "Unknown Error"}";
		}

		public async Task<string> UpdateLeaveTransaction(LeaveTransactionDto leaveTransaction, int id)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var jsonContent = JsonConvert.SerializeObject(leaveTransaction);
			var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(apiCredential.url + "LeaveTransaction/UpdateLeaveTransaction/" + id, contentString);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (responseModel != null && response.IsSuccessStatusCode)
			{
				return "Leave Updated successfully";
			}
			else
			{
				return "Leave Updated Failed";
			}
		}
		public async Task<int> DeleteLeaveTransaction(int id)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var response = await _httpClient.DeleteAsync(apiCredential.url + "LeaveTransaction/DeleteLeaveTransaction/" + id);
			if (response.IsSuccessStatusCode)
			{
				return id;
			}
			return 0;
		}
		public async Task<List<LeaveTransactionDto>> UpdateStatusAsync(List<int?> leaveTransactionIds)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var jsonContent = JsonConvert.SerializeObject(leaveTransactionIds);
			var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(apiCredential.url + "LeaveTransaction/UpdateLeaveTransactionStatus", contentString);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (responseModel != null)
			{
				var details = JsonConvert.DeserializeObject<List<LeaveTransactionDto>>(Convert.ToString(responseModel.Data!));
				if (jsonContent != null && jsonContent.Count() > 0)
				{
					return details;
				}
			}
			return null;
		}
	}
}
