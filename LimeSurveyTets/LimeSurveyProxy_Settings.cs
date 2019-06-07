using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public Task<RPCResponse> GetSiteSettings(string settingName) =>
            RequestAuthRPC(
                "get_site_settings",
                ("sSetttingName", settingName)
                );
    }
}
