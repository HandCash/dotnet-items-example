using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace HandcashClient.Tests
{
    public class HandcashClientTestHelper
    {
        public static HandcashClient CreateHandcashClient()
        {
            var mockHttpClient = new Mock<HttpClient>();
            var mockLogger = new Mock<ILogger<HandcashClient>>();

            var configuration = LoadConfiguration();
            var config = new HandcashClientConfig
            {
                AppSecret = configuration["HandcashConfig:AppSecret"],
                AppId = configuration["HandcashConfig:AppId"],
                BaseUrl = configuration["HandcashConfig:BaseUrl"],
                BusinessWalletAuthToken = configuration["HandcashConfig:AuthToken"]
            };

            return new HandcashClient(mockHttpClient.Object, mockLogger.Object, config);
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
