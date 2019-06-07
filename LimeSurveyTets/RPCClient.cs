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

    public class LimeSurveyProxy
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

        public async Task<RPCResponse> Login()
        {
            var result = await RequestRPC(
                "get_session_key",
                ("username", _username),
                ("password", _password)
                );
            _sessionKey = result.Result.ToString();

            return result;
        }

        public Task<RPCResponse> ImportSurvey(int id, string name, string data, string type) =>
            RequestAuthRPC(
                "import_survey",
                ("sImportData", EncodeString(data)),
                ("sImportDataType", type),
                ("sNewSurveyName", name),
                ("DestSurveyID", id)
                );

        public async Task<RPCResponse> ImportSurvey(int id, string name, Stream data, string type)
        {
            var buffer = new Memory<byte>();
            _ = await data.ReadAsync(buffer);
            return await ImportSurvey(
                id,
                name,
                _encoding.GetString(buffer.ToArray()),
                type
                );
        }

        public Task<RPCResponse> ImportGroup(int surveyId, string importData, string dataType, string groupName, string groupDescription) =>
            RequestAuthRPC(
                "import_group",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sImportData", importData),
                ("sImportDataType", dataType),
                ("sNewGroupName", groupName),
                ("sNewGroupDescription", groupDescription)
                );

        public Task<RPCResponse> ImportQuestion(int surveyId, int groupId, string data, string dataType, string questionTitle, bool isMandatory = false) =>
            RequestAuthRPC(
                "import_question",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("iGroupID", JToken.FromObject(groupId)),
                ("sImportData", data),
                ("sImportDataType", dataType),
                ("sNewQuestionTitle", questionTitle),
                ("sMandatory", isMandatory ? 'Y' : 'N')
                );

        public Task<RPCResponse> AddSurvey(int surveyId, string title, string language) =>
            RequestAuthRPC(
                "add_survey",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sSurveyTitle", title),
                ("sSurveyLanguage", language)
                );

        public Task<RPCResponse> DeleteSurvey(int surveyId) =>
            RequestAuthRPC(
                "delete_survey",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

        public Task<RPCResponse> ActivateSurvey(int surveyId) =>
            RequestAuthRPC(
                "activate_survey",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

        public Task<RPCResponse> AddGroup(int surveyId, string groupTitle, string groupDescription) =>
            RequestAuthRPC(
                "add_group",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sGroupTitle", groupTitle),
                ("sGroupDescription", groupDescription)
                );

        public async Task<SurveySummary> GetSummary(int surveyId)
        {
            var parameters = ConstructParameters(("iSurveyID", JToken.FromObject(surveyId)));
            var response = await Post(
                CreateAuthRPCObject("get_summary", parameters)
                );
            var result = await ConvertResponse(response);

            return DeserializeString<SurveySummary>(result.Result.ToString());
        }

        public Task<RPCResponse> GetSiteSettings(string settingName) =>
            RequestAuthRPC(
                "get_site_settings",
                ("sSetttingName", settingName)
                );

        public async Task<SurveyInfo> GetSurveyProperties(int surveyId)
        {
            var result = await RequestAuthRPC(
                "get_survey_properties",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

            return DeserializeString<SurveyInfo>(result.Result.ToString());
        }

        public async Task<GroupInfo> GetGroupProperties(int groupId)
        {
            var result = await RequestAuthRPC(
                "get_group_properties",
                ("iGroupID", JToken.FromObject(groupId))
                );

            return DeserializeString<GroupInfo>(result.Result.ToString());
        }

        public async Task<LanguageInfo> GetLanguageProperties(int surveyId)
        {
            var result = await RequestAuthRPC(
                "get_language_properties",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

            return DeserializeString<LanguageInfo>(result.Result.ToString());
        }

        public async Task<QuestionInfo> GetQuestionProperties(int questionId)
        {
            var result = await RequestAuthRPC(
                "get_question_properties",
                ("iQuestionID", JToken.FromObject(questionId))
                );

            return DeserializeString<QuestionInfo>(result.Result.ToString());
        }

        public Task<RPCResponse> UploadFile(int surveyId, string fieldName, string fileName, string fileContent) =>
            RequestAuthRPC(
                "upload_file",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sFieldName", fieldName),
                ("sFileName", fileName),
                ("sFileContent", fileContent)
                );

        public Task<RPCResponse> SetSurveyProperties(int surveyId, SurveyInfo properties) =>
            RequestAuthRPC(
                "set_survey_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aSurveyData", JToken.FromObject(properties))
                );

        public Task<RPCResponse> SetLanguageProperties(int surveyId, LanguageInfo properties) =>
            RequestAuthRPC(
                "set_language_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aSurveyLocaleData", JToken.FromObject(properties))
                );

        public Task<RPCResponse> SetLanguageProperties(int surveyId, object properties) =>
            RequestAuthRPC(
                "set_language_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aSurveyLocaleData", JToken.FromObject(properties))
                );

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
