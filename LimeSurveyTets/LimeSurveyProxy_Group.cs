using LimeSurveyTest.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<IEnumerable<GroupInfo>> ListGroups(int surveyId)
        {
            var result = await RequestAuthRPC(
                "list_groups",
                ("iSurveyID", JToken.FromObject(surveyId))
                );

            return DeserializeString<IEnumerable<GroupInfo>>(result.Result.ToString());
        }

        public Task<RPCResponse> ImportGroup(int surveyId, string importData, string dataType, string groupName, string groupDescription) =>
            RequestAuthRPC(
                "import_group",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sImportData", EncodeString(importData)),
                ("sImportDataType", dataType),
                ("sNewGroupName", groupName),
                ("sNewGroupDescription", groupDescription)
                );

        public Task<RPCResponse> AddGroup(int surveyId, string groupTitle, string groupDescription) =>
            RequestAuthRPC(
                "add_group",
                ("iSurveyID", JToken.FromObject(surveyId)),
                ("sGroupTitle", groupTitle),
                ("sGroupDescription", groupDescription)
                );

        public async Task<GroupInfo> GetGroupProperties(int groupId)
        {
            var result = await RequestAuthRPC(
                "get_group_properties",
                ("iGroupID", JToken.FromObject(groupId))
                );

            return DeserializeString<GroupInfo>(result.Result.ToString());
        }
    }
}
