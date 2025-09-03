using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Attendance.UI.Domain.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Attendance.Infrastructure.Efcore.Providers
{
    public class AttendanceAdaptor : IAttendanceAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public AttendanceAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(configuration);
        }

        public async Task<AttendanceRecordDto> ClockInAsync()
        {
            try
            {
                var _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
                var response = await _httpClient.GetAsync(apiCredential.url + "Attendance/ClockIn");
                var responseData = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
                if (responseModel != null && response.IsSuccessStatusCode)
                {
                    var details = JsonConvert.DeserializeObject<AttendanceRecordDto>(Convert.ToString(responseModel.Data!));
                    return details;
                }
                else
                {
                    var details = JsonConvert.DeserializeObject<CommonFailureDto>(Convert.ToString(responseModel.Data!));
                    throw new Exception(details.Error);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<AttendanceRecordDto> ClockOutAsync()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "Attendance/ClockOut");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<AttendanceRecordDto>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }

        public async Task<List<AttendanceRecordDto>> GetAttendanceByEmployeeAsync(int employeeId)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "Attendance/Report?employeeID" + employeeId);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<AttendanceRecordDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        public async Task<List<AttendanceRecordDto>> GetLastFiveAttendanceRecordsAsync(int employeeId)
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "Attendance/Report?employeeID" + employeeId);
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<AttendanceRecordDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
        public async Task<bool> IsUserClockedIn()
        {
            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "Attendance/IsUserClockedIn");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<bool>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return false;
        }
    }
}
