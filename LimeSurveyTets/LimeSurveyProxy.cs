using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LimeSurveyTest
{

    public partial class LimeSurveyProxy
    {
        private int idCounter;

        public string Username { get; }
        public Uri Uri { get; }

        private string _password;
        private string _sessionKey;
        private readonly string _dataType;
        private readonly Encoding _encoding;
        private readonly HttpClient _client;
        private readonly DefaultContractResolver _contractResolver;
        private readonly JsonSerializerSettings _serializerSettings;

        public LimeSurveyProxy(Uri uri, string username, string password)
            : this(Encoding.UTF8, new SnakeCaseNamingStrategy())
        {
            Username = username;
            _password = password;
            Uri = uri;
        }

        protected LimeSurveyProxy(Encoding encoding, NamingStrategy naming)
        {
            _client = new HttpClient();
            _dataType = "application/json";
            _encoding = Encoding.UTF8;
            _contractResolver = new DefaultContractResolver
            {
                NamingStrategy = naming
            };
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = _contractResolver,
                Formatting = Formatting.Indented
            };
        }

        public async Task<RPCResponse> RequestAuthRPC(string rpcMethod, params (string propertyName, JToken value)[] rpcParams) =>
            await ConvertResponse(
                await Post(
                    CreateAuthRPCObject(
                        rpcMethod,
                        ConstructParameters(rpcParams)
                        )
                    )
                );

        public async Task<RPCResponse> RequestRPC(string rpcMethod, params (string propertyName, JToken value)[] rpcParams) =>
            await ConvertResponse(
                await Post(
                    CreateRPCObject(
                        rpcMethod,
                        ConstructParameters(rpcParams)
                        )
                    )
                );

        protected Task<HttpResponseMessage> Post(object value) =>
            _client.PostAsync(
                Uri,
                new StringContent(
                    SerializeObject(value),
                    _encoding,
                    _dataType
                    ));

        public string EncodeString(string str) =>
            Convert.ToBase64String(_encoding.GetBytes(str));

        public string DecodeString(string str) =>
            _encoding.GetString(Convert.FromBase64String(str));

        protected async Task<RPCResponse> ConvertResponse(HttpResponseMessage response)
        {
            var rpc = DeserializeString<RPCResponse>(
                await response.Content.ReadAsStringAsync()
                );

            var result = rpc.Result.ToString();
            rpc.Status =
                result.Contains("\"status\"")
                ? JToken.Parse(result)["status"]?.ToString()
                : string.Empty;

            return rpc;
        }

        protected JObject CreateRPCObject(string method, JObject parameters) =>
            ConstructParameters(
                ("jsonrpc", "2.0"),
                ("id", ++idCounter),
                ("method", method),
                ("params", parameters)
                );

        public JObject ConstructParameters(params (string propertyName, JToken value)[] jParams)
        {
            var parameters = new JObject();
            foreach (var param in jParams)
                parameters.Add(
                    new JProperty(param.propertyName, param.value)
                    );
            return parameters;
        }

        protected JObject CreateAuthRPCObject(string method, JObject parameters)
        {
            parameters.AddFirst(new JProperty("sSessionKey", _sessionKey));
            return CreateRPCObject(method, parameters);
        }

        protected string SerializeObject(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, _serializerSettings);
            }
            catch(JsonSerializationException) when (obj != null)
            {
                return null;
            }
        }

        protected T DeserializeString<T>(string str) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str, _serializerSettings);
            }
            catch (JsonSerializationException) when (!string.IsNullOrEmpty(str))
            {
                return null;
            }
        }
    }
}
