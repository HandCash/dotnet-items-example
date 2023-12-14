using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using NBitcoin;
using NBitcoin.DataEncoders;
using HandcashClient.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Serialization;



namespace HandcashClient
{
    public class HandcashClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<HandcashClient> _logger;
        private readonly HandcashClientConfig _config;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        private readonly string _businessWalletAuthToken;

        public HandcashClient(HttpClient client, ILogger<HandcashClient> logger, HandcashClientConfig config)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _businessWalletAuthToken = _config.BusinessWalletAuthToken;
            ConfigureHttpClient();

            _jsonSerializerOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        }

        private void ConfigureHttpClient()
        {
            _client.BaseAddress = new Uri(_config.BaseUrl);
            _client.DefaultRequestHeaders.Add("app-secret", _config.AppSecret);
            _client.DefaultRequestHeaders.Add("app-id", _config.AppId);
        }
        
        private (string signature, string publicKey) GetSignatureAndPublicKey(byte[] hash, string authToken)
        {
            var privateKey = new Key(Encoders.Hex.DecodeData(authToken));
            var publicKey = privateKey.PubKey;
            var signature = privateKey.Sign(new uint256(hash)).ToDER();

            return (Encoders.Hex.EncodeData(signature), publicKey.ToString());
        }

        private byte[] GetRequestSignatureHash(string method, string endpoint, object body, string timestamp, string nonce = "")
        {
            string bodyString = body != null ? JsonSerializer.Serialize(body, _jsonSerializerOptions) : "{}";
            string signatureString = $"{method}\n{endpoint}\n{timestamp}\n{bodyString}{(string.IsNullOrEmpty(nonce) ? "" : $"\n{nonce}")}";
            byte[] signatureBytes = Encoding.UTF8.GetBytes(signatureString);

            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(signatureBytes);
            }
        }
        public async Task<HttpResponseMessage> DefaultRequest(string method, string url, object body, string authToken)
        {
            var timestamp = DateTime.UtcNow.ToString("o");
            var signatureHash = GetRequestSignatureHash(method, url, body, timestamp);
            var (signature, publicKey) = GetSignatureAndPublicKey(signatureHash, authToken);

            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(url, UriKind.Relative)
            };

            requestMessage.Headers.Add("oauth-signature", signature);
            requestMessage.Headers.Add("oauth-timestamp", timestamp);
            requestMessage.Headers.Add("oauth-publickey", publicKey);

            string bodyString = body != null ? JsonSerializer.Serialize(body, _jsonSerializerOptions) : "{}";
            requestMessage.Content = new StringContent(bodyString, Encoding.UTF8, "application/json");

            return await _client.SendAsync(requestMessage);
        }

       public async Task<CreateItemsOrder> CreateCollectionItemOrder(string collectionId, List<CreateItemMetadata> itemsToCreate)
       {
            var requestUri = "/v3/itemCreationOrder/issueItems";

            var requestBody = new
            {
                items = itemsToCreate,
                itemCreationOrderType = "collectionItem",
                referencedCollection = collectionId
            };

            var response = await DefaultRequest("POST", requestUri, requestBody, _businessWalletAuthToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObj = JsonSerializer.Deserialize<CreateItemsOrder>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseObj != null)
                {
                    return responseObj;
                }
                else
                {
                    throw new JsonException("Failed to deserialize response content.");
                }
            }
            else
            {
                throw new HttpRequestException($"Unexpected server response: {response.StatusCode}");
            }
        }


        public async Task<CreateItemsOrder> GetOrderStatus(string orderId)
        {
            var requestUri = $"/v3/itemCreationOrder/{orderId}";

            var response = await DefaultRequest("GET", requestUri, null, _businessWalletAuthToken);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var orderStatus = await response.Content.ReadFromJsonAsync<CreateItemsOrder>(options);
                return orderStatus;
            }
            else
            {
                throw new HttpRequestException($"Unexpected server response: {response.StatusCode}");
            }

        }


        private class ItemResponse
        {
            public List<Item> items { get; set; }
        }
        public async Task<List<Item>> GetItemsByOrder(string orderId)
        {
            var requestUri = $"/v3/itemCreationOrder/{orderId}/items";

            var response = await DefaultRequest("GET", requestUri, null, _businessWalletAuthToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ItemResponse>(responseContent);

                return responseObject?.items ?? new List<Item>();
            }
            else
            {
                throw new HttpRequestException($"Unexpected server response: {response.StatusCode}");
            }
        }
    }
}
