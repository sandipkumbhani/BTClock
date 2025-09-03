using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.UI.Domain.Helper
{
    public class APICredential
    {
        public string? url { get; set; }
        public APICredential(IConfiguration configuration)
        {
            var qurtzsetting = configuration.GetSection("APICredential");
            url = qurtzsetting.GetSection("url").Value;
        }
    }
    
}
