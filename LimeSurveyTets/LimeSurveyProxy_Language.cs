using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<LanguageInfo> GetLanguageProperties(int surveyId)
        {
            var result = await RequestAuthRPC(
                "get_language_properties",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

            return DeserializeString<LanguageInfo>(result.Result.ToString());
        }

        public Task<RPCResponse> SetLanguageProperties(int surveyId, LanguageInfo properties) =>
            RequestAuthRPC(
                "set_language_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aSurveyLocaleData", JToken.FromObject(properties))
                );
    }
}
