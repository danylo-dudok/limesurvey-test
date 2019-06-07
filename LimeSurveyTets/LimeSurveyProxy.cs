using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LimeSurveyTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace LimeSurveyTest
{

    public partial class LimeSurveyProxy
    {
        private int idCounter;
        private string _username;
        private string _password;
        private string _sessionKey;
        private readonly string _dataType;
        private readonly Encoding _encoding;
        private Uri Uri { get; }
        private readonly HttpClient _client;
        private readonly DefaultContractResolver _contractResolver;
        private readonly JsonSerializerSettings _serializerSettings;

        public LimeSurveyProxy(Uri uri, string username, string password)
        {
            _username = username;
            _password = password;
            Uri = uri;
            _client = new HttpClient();
            _dataType = "application/json";
            _encoding = Encoding.UTF8;
            _contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = _contractResolver,
                Formatting = Formatting.Indented
            };
        }

        private async Task<RPCResponse> RequestAuthRPC(string rpcMethod, params (string propertyName, JToken value)[] rpcParams) =>
            await ConvertResponse(
                await Post(
                    CreateAuthRPCObject(
                        rpcMethod,
                        ConstructParameters(rpcParams)
                        )
                    )
                );

        private async Task<RPCResponse> RequestRPC(string rpcMethod, params (string propertyName, JToken value)[] rpcParams) =>
            await ConvertResponse(
                await Post(
                    CreateRPCObject(
                        rpcMethod,
                        ConstructParameters(rpcParams)
                        )
                    )
                );

        private Task<HttpResponseMessage> Post(object value) =>
            _client.PostAsync(
                Uri,
                new StringContent(
                    SerializeObject(value),
                    _encoding,
                    _dataType
                    ));

        private string EncodeString(string str) =>
            Convert.ToBase64String(_encoding.GetBytes(str));

        private async Task<RPCResponse> ConvertResponse(HttpResponseMessage response) =>
            DeserializeString<RPCResponse>(
                await response.Content.ReadAsStringAsync()
                );

        private JObject CreateRPCObject(string method, JObject parameters) =>
            ConstructParameters(
                ("jsonrpc", "2.0"),
                ("id", ++idCounter),
                ("method", method),
                ("params", parameters)
                );

        private JObject ConstructParameters(params (string propertyName, JToken value)[] jParams)
        {
            var parameters = new JObject();
            foreach (var param in jParams)
                parameters.Add(
                    new JProperty(param.propertyName, param.value)
                    );
            return parameters;
        }

        private JObject CreateAuthRPCObject(string method, JObject parameters)
        {
            parameters.AddFirst(new JProperty("sSessionKey", _sessionKey));
            return CreateRPCObject(method, parameters);
        }

        private string SerializeObject(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, _serializerSettings);
            }
            catch(JsonReaderException ex) when (obj != null)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private T DeserializeString<T>(string str) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str, _serializerSettings);
            }
            catch (JsonWriterException ex) when (!string.IsNullOrEmpty(str))
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
