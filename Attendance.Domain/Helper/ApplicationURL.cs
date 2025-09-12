using Microsoft.Extensions.Configuration;

namespace Attendance.UI.Domain.Helper
{
    public class ApplicationURL
    {
        public string? url { get; set; }
        public ApplicationURL(IConfiguration configuration)
        {
            var qurtzsetting = configuration.GetSection("ApplicationUrl");
            url = qurtzsetting.GetSection("url").Value;
        }
    }
}
