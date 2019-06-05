using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonRPCClient
{

    public class LimeSurveyProxy
    {
        private readonly JProperty jsonRPC = new JProperty("jsonrpc", "2.0");
        private int idCounter;
        private string _username;
        private string _password;
        private string _sessionKey;
        private readonly string _dataType;
        private readonly Encoding _encoding;
        private Uri Uri {get;}
        private HttpClient _client;

        public LimeSurveyProxy(Uri uri, string username, string password)
        {
            _username = username;
            _password = password;
            Uri = uri;
            _client = new HttpClient();
            _dataType = "application/json";
            _encoding = Encoding.UTF8;
        }

        public async Task<HttpStatusCode> Login()
        {
            var credentials = new JObject();
            credentials.Add("username", _username);
            credentials.Add("password", _password);
            var response = await _client.PostAsync(
                Uri,
                new StringContent(
                    JsonConvert.SerializeObject(
                        CreatePostObject("get_session_key", credentials)
                        ),
                    _encoding,
                    _dataType
                    ));
            _sessionKey = JsonConvert.DeserializeObject<JsonRPCResponse>(
                await response.Content.ReadAsStringAsync()).Result.ToString();

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> ImportSurvey(int id, string name, string data, string type = "text")
        {
            var parameters = new JObject();
            parameters.Add("sSessionKey", _sessionKey);
            parameters.Add("sImportData", Convert.ToBase64String(Encoding.UTF8.GetBytes(data)));
            parameters.Add("sImportDataType", type);
            parameters.Add("sNewSurveyName", name);
            parameters.Add("DestSurveyID", id);

            var response = await _client.PostAsync(
                Uri,
                new StringContent(
                    JsonConvert.SerializeObject(
                        CreatePostObject("import_survey", parameters)
                        ),
                    _encoding,
                    _dataType
                    ));

            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> ImportSurvey(int id, string name, Stream data, string type)
        {
            var buffer = new Memory<byte>();
            _ = await data.ReadAsync(buffer);
            return await ImportSurvey(id, name, Encoding.UTF8.GetString(buffer.ToArray()), type);
        }

        private JObject CreatePostObject(string method, JObject parameters)
        {
            var jobject = new JObject();

            jobject.Add(new JProperty("jsonrpc", "2.0"));
            jobject.Add(new JProperty("id", ++idCounter));
            jobject.Add(new JProperty("method", method));
            jobject.Add(new JProperty("params", parameters));

            return jobject;
        }
    }

    public class JsonRPCResponse
    {
        public int Id { set; get; }
        public object Result { set; get; }
        public string Error { set; get; }
    }
}
