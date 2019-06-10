using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<RPCResponse> GetResponseIds(int surveyId, string token)
        {
            var result = await RequestAuthRPC(
                "get_response_ids",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sToken", token)
                );

            return result;
        }

        public async Task<RPCResponse> AddResponse(int surveyId, ResponseInfo data)
        {
            var result = await RequestAuthRPC(
                "add_response",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aResponseData", JToken.FromObject(data))
                );

            return result;
        }

        public async Task<IEnumerable<ResponseInfo>> ExportResponses(int surveyId)
        {
            var result = await RequestAuthRPC(
                "export_responses",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sDocumentType", "json")
                );

            var json = result.Result.ToString().Trim();

            if (string.IsNullOrEmpty(json))
                return null;

            json = DecodeString(json);
            return JObject.Parse(json)
                .Property("responses")
                .Select(x => x.Children().First())
                .Select(x => x.ToObject<IDictionary<int, ResponseInfo>>())
                .SelectMany(x => x.Values);
        }

        public async Task<RPCResponse> ExportStatistic(int surveyId)
        {
            var result = await RequestAuthRPC(
                "export_statistics",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

            result.Result = DecodeString(result.Result.ToString());

            return result;
        }
    }
}
