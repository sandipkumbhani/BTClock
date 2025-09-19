using Attendance.Domain.Helper;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class LeaveBalanceAdaptor : ILeaveBalanceAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public LeaveBalanceAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<List<LeaveBalanceDto>> GetAllLeaveBalances()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);  
            var response = await _httpClient.GetAsync(apiCredential.url + "LeaveBalance/GetAllLeaveBalances");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LeaveBalanceDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        
        public async Task<IEnumerable<LeaveBalanceDto>> GetLeaveBalanceByEmployeeId(int employeeId)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "LeaveBalance/GetLeaveBalanceByEmployeeId/" + employeeId);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LeaveBalanceDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        public async Task<LeaveBalanceDto> UpsertLeaveBalance(LeaveBalanceDto leaveBalance)
		{
			var _httpClient = new HttpClient();
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
			var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(leaveBalance), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(apiCredential.url + "LeaveBalance/UpsertLeaveBalance", content);
			var responseData = await response.Content.ReadAsStringAsync();
			var responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
			if (responseModel != null)
			{
				var details = Newtonsoft.Json.JsonConvert.DeserializeObject<LeaveBalanceDto>(Convert.ToString(responseModel.Data!));
				return details;
			}
			return null;
		}
	}
}
