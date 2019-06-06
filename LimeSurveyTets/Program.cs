using System;
using static System.Console;
using System.IO;
using Newtonsoft.Json;
using LimeSurveyTest.Models;

namespace LimeSurveyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new LimeSurveyProxy(new Uri("http://localhost/index.php?r=admin/remotecontrol"), "admin", "admin");
            WriteLine(proxy.Login().GetAwaiter().GetResult().Result);
            var info = proxy.GetSurveyProperties(4).GetAwaiter().GetResult();
            WriteLine(JsonConvert.SerializeObject(info));
            info.Active = "N";
            WriteLine(proxy.SetSurveyProperties(4, info).GetAwaiter().GetResult().Result);
            //WriteLine(JsonConvert.SerializeObject(proxy.GetSurveyProperties(4).GetAwaiter().GetResult()));
            //WriteLine(proxy.ActivateSurvey(4).GetAwaiter().GetResult().Result);
            //WriteLine(proxy.DeleteSurvey(2).GetAwaiter().GetResult().Result);
            //WriteLine(proxy.AddSurvey(4, "hello earthians", "en").GetAwaiter().GetResult().Result);
            //WriteLine(proxy.GetSummary(4).GetAwaiter().GetResult().Result);
            //WriteLine(proxy.ImportSurvey(2, "test10", File.ReadAllText("../../../Limesurvey_sample_survey_Assessment.lss"), "lss").GetAwaiter().GetResult().Result);
            ReadKey();
        }
    }
}
