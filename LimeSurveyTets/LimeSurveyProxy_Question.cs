using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
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

        public async Task<QuestionInfo> GetQuestionProperties(int questionId)
        {
            var result = await RequestAuthRPC(
                "get_question_properties",
                ("iQuestionID", JToken.FromObject(questionId))
                );

            return DeserializeString<QuestionInfo>(result.Result.ToString());
        }
    }
}
