using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<IEnumerable<SurveyInfo>> ListSurveys()
        {
            var result = await RequestAuthRPC("list_surveys");

            return DeserializeString<IEnumerable<SurveyInfo>>(result.Result.ToString());
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

        public Task<RPCResponse> AddSurvey(int surveyId, string title, string language) =>
            RequestAuthRPC(
                "add_survey",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sSurveyTitle", title),
                ("sSurveyLanguage", language)
                );

        public Task<RPCResponse> CopySurvey(int surveyId, string newName) =>
            RequestAuthRPC(
                "add_survey",
                ("iSurveyID_org", JToken.FromObject(surveyId)),
                ("sNewname", newName)
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

        public async Task<SurveyInfo> GetSurveyProperties(int surveyId)
        {
            var result = await RequestAuthRPC(
                "get_survey_properties",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

            return DeserializeString<SurveyInfo>(result.Result.ToString());
        }

        public Task<RPCResponse> SetSurveyProperties(int surveyId, SurveyInfo properties) =>
            RequestAuthRPC(
                "set_survey_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aSurveyData", JToken.FromObject(properties))
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
    }
}
