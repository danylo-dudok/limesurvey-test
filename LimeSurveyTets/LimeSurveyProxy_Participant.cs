using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<ParticipantInfo> GetParticipantProperties(int surveyId, int participantId)
        {
            var result = await RequestAuthRPC(
                "get_participant_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aTokenQueryProperties", JToken.FromObject(participantId))
                );

            return DeserializeString<ParticipantInfo>(result.Result.ToString());
        }

        public async Task<RPCResponse> SetParticipantProperties(int surveyId, int participantId, ParticipantInfo data)
        {
            var result = await RequestAuthRPC(
                "set_participant_properties",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("aTokenQueryProperties", JToken.FromObject(participantId)),
                ("aTokenData", JToken.FromObject(data))
                );

            return result;
        }

        public async Task<IEnumerable<ParticipantCredentials>> ListParticipants(int surveyId, int offset, int amount)
        {
            var result = await RequestAuthRPC(
                "list_participants",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("iStart", JToken.FromObject(offset)),
                ("iLimit", JToken.FromObject(amount)),
                ("bUnused", JToken.FromObject(true))
                );
        
            return DeserializeString<IEnumerable<ParticipantCredentials>>(result.Result.ToString());
        }
    }
}
