using RestSharp;
namespace MyShop.Configs
{
    public class RestSharpClient
    {
        private static RestClient _client;
        private static RestClientOptions _options = new RestClientOptions(Constant.BACKEND_URL)
        {
            ThrowOnDeserializationError = true,
        };

        private RestSharpClient() { }

        public static RestClient getInstance()
        {
            if (_client == null)
            {
                _client = new RestClient(_options);
            }
            return _client;
        }
    }
}
