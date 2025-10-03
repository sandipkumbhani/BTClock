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
    public class RoleAdaptor : IRoleAdaptor
    {
        private readonly GlobalClass _globalClass;
        private readonly IConfiguration _configuration;
        private APICredential apiCredential;

        public RoleAdaptor(GlobalClass globalClass, IConfiguration configuration)
        {
            _globalClass = globalClass;
            _configuration = configuration;
            apiCredential = new APICredential(_configuration);
        }
        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {

            var _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _globalClass.Token);
            var response = await _httpClient.GetAsync(apiCredential.url + "Role/GetAllRole");
            var responseData = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<CommanResponseDto>(responseData);
            if (responseModel != null)
            {
                var details = JsonConvert.DeserializeObject<List<RoleDto>>(Convert.ToString(responseModel.Data!));
                return details;
            }
            return null;
        }
    }
}
