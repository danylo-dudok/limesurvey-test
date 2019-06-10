using System.Threading.Tasks;

namespace LimeSurveyTest
{
    public partial class LimeSurveyProxy
    {
        public async Task<RPCResponse> Login()
        {
            var result = await RequestRPC(
                "get_session_key",
                ("username", Username),
                ("password", _password)
                );
            _sessionKey = result.Result.ToString();

            return result;
        }
    }
}
