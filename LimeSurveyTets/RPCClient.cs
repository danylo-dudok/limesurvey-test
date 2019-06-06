﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public async Task<RPCResponse> Login()
        {
            var result = await RequestAuthRPC(
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
                ("iSurveyID", surveyId.ToString()),
                ("sImportData", importData),
                ("sImportDataType", dataType),
                ("sNewGroupName", groupName),
                ("sNewGroupDescription", groupDescription)
                );

        public Task<RPCResponse> AddSurvey(int surveyId, string title, string language) =>
            RequestAuthRPC(
                "add_survey",
                ("iSurveyID", surveyId.ToString()),
                ("sSurveyTitle", title),
                ("sSurveyLanguage", language)
                );

        public Task<RPCResponse> DeleteSurvey(int surveyId) =>
            RequestAuthRPC(
                "delete_survey",
                ("iSurveyID", surveyId.ToString())
                );

        public Task<RPCResponse> ActivateSurvey(int surveyId) =>
            RequestAuthRPC(
                "activate_survey",
                ("iSurveyID", surveyId.ToString())
                );

        public Task<RPCResponse> AddGroup(int surveyId, string groupTitle, string groupDescription) =>
            RequestAuthRPC(
                "add_group",
                ("iSurveyID", surveyId.ToString()),
                ("sGroupTitle", groupTitle),
                ("sGroupDescription", groupDescription)
                );

        public async Task<RPCResponse> GetSummary(int surveyId)
        {
            var parameters = ConstructParameters(("iSurveyID", surveyId.ToString()));
            var response = await Post(
                CreateAuthRPCObject("get_summary", parameters)
                );
            return await ConvertResponse(response);
        }

        public Task<RPCResponse> GetSiteSettings(string settingName) =>
            RequestAuthRPC(
                "get_site_settings",
                ("sSetttingName", settingName)
                );

        public Task<RPCResponse> UploadFile(int surveyId, string fieldName, string fileName, string fileContent) =>
            RequestAuthRPC(
                "upload_file",
                ("iSurveyID", surveyId.ToString()),
                ("sFieldName", fieldName),
                ("sFileName", fileName),
                ("sFileContent", fileContent)
                );

        public Task<RPCResponse> SetSurveyProperties(int surveyId, params (string propertyName, string value)[] surveyParams) =>
            RequestAuthRPC(
                "set_survey_properties",
                ("iSurveyID", surveyId.ToString()),
                (
                    "aSurveyData",
                    ConstructParameters(
                        surveyParams.Select(x =>
                            (x.propertyName, JToken.Parse(x.value))
                            ).ToArray()
                        )
                ));

        //public Task<RPCResponse> SetQuestionProperties(int surveyId, params (string propertyName, string value)[] surveyParams) =>
        //    RequestAuthRPC(
        //        "upload_file",
        //        ("iSurveyID", surveyId.ToString()),
        //        (
        //            "aSurveyData",
        //            ConstructParameters(
        //                surveyParams.Select(x =>
        //                    (x.propertyName, JToken.Parse(x.value))
        //                    ).ToArray()
        //                )
        //        ));

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
                    JsonConvert.SerializeObject(value),
                    _encoding,
                    _dataType
                    ));

        private string EncodeString(string str) =>
            Convert.ToBase64String(_encoding.GetBytes(str));

        private async Task<RPCResponse> ConvertResponse(HttpResponseMessage response) =>
            JsonConvert.DeserializeObject<RPCResponse>(
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
    }
}
