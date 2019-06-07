using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<IEnumerable<ParticipantInfo>> GetParticipantProperties(int surveyId, int tokenQuery)
        {
            var result = await RequestAuthRPC(
                "get_participant_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aTokenQueryProperties", JToken.FromObject(tokenQuery))
                );

            return DeserializeString<IEnumerable<ParticipantInfo>>(result.Result.ToString());
        }
    }
}
