using NETCore.MailKit.Infrastructure.Internal;

namespace Authentication.Models.Configuration
{
    public class Application
    {
        public MailKitOptions MailSettings { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}