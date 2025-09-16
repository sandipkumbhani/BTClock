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
    public class LeaveAssignmentAdaptor : ILeaveAssignmentAdaptor
    {

        private readonly IConfiguration _configuration;
        private APICredential apiCredential;
        private readonly GlobalClass _globalClass;

        public LeaveAssignmentAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }
        public async Task<List<LeaveAssignmentDto>> GetAllLeaveAssignments()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "LeaveAssignment/GetAllLeaveAssignments");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<LeaveAssignmentDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null!;
        }
        public async Task<LeaveAssignmentDto> GetLeaveAssignmentById(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "LeaveAssignment/GetLeaveAssignmentById/" + id);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<LeaveAssignmentDto>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null!;
        }
        public async Task<string> AddLeaveAssignment(LeaveAssignmentDto leaveAssignment)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var content = new StringContent(JsonConvert.SerializeObject(leaveAssignment), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiCredential.url + "LeaveAssignment/AddLeaveAssignment", content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null && response.IsSuccessStatusCode)
            {
                return "LeaveAssign Added Successfully";
            }
            else
            {
                return "LeaveAssign Not Added";
            }
        }
        public async Task<string> UpdateLeaveAssignment(LeaveAssignmentDto leaveAssignment, int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var content = new StringContent(JsonConvert.SerializeObject(leaveAssignment), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiCredential.url + "LeaveAssignment/UpdateLeaveAssignment/" + id, content);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null && response.IsSuccessStatusCode)
            {
                return "LeaveAssign Updated Successfully";
            }
            else
            {
                return "LeaveAssign Not Updated";
            }
                
        }
        public async Task<int> DeleteLeaveAssignment(int id)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.DeleteAsync(apiCredential.url + "LeaveAssignment/DeleteLeaveAssignment/" + id);
            if (response.IsSuccessStatusCode)
            {
                return id;
            }
            return 0;
        }
    }
}
