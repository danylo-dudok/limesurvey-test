using System;
using System.Text;
using System.Net;
using static System.Console;
using System.IO;
using JsonRPCClient;

namespace LimeSurveyTets
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new LimeSurveyProxy(new Uri("http://localhost/index.php?r=admin/remotecontrol"), "admin", "admin");
            WriteLine(proxy.Login().GetAwaiter().GetResult());
            WriteLine(proxy.ImportSurvey(2, "test10", File.ReadAllText("../../../Limesurvey_sample_survey_Assessment.lss"), "lss").GetAwaiter().GetResult());
            ReadKey();
        }
    }
}
