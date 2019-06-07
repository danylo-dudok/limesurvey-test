using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public Task<RPCResponse> UploadFile(int surveyId, string fieldName, string fileName, string fileContent) =>
            RequestAuthRPC(
                "upload_file",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sFieldName", fieldName),
                ("sFileName", fileName),
                ("sFileContent", fileContent)
                );
    }
}
