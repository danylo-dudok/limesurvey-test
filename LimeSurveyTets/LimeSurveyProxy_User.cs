using LimeSurveyTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<IEnumerable<UserInfo>> ListUsers()
        {
            var result = await RequestAuthRPC("list_users");

            return DeserializeString<IEnumerable<UserInfo>>(result.Result.ToString());
        }
    }
}
