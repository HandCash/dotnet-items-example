public class HandcashClientConfig
{
    public string BaseUrl { get; set; } = "https://iae.cloud.handcash.io"; // Default value
    public string AppSecret { get; set; }
    public string AppId { get; set; }

    public string BusinessWalletAuthToken { get; set; }
}
