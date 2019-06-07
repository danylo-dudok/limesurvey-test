using System;
using static System.Console;
using System.IO;
using Newtonsoft.Json;
using LimeSurveyTest.Models;

namespace LimeSurveyTest
{
    class Program
    {
        // vezoctopmailer
        // qwerty12345
        static void Main(string[] args)
        {
            //var proxy = new LimeSurveyProxy(new Uri("https://helloworld.limequery.com/admin/remotecontrol"), "vezoctopmailer", "qwerty12345");
            var proxy = new LimeSurveyProxy(new Uri("http://localhost/index.php?r=admin/remotecontrol"), "admin", "admin");
            WriteLine(proxy.Login().GetAwaiter().GetResult().Result);
            var lang = proxy.GetLanguageProperties(4).GetAwaiter().GetResult();
            lang.SurveylsDescription = "This is my description to api";
            WriteLine(proxy.SetLanguageProperties(4, lang).GetAwaiter().GetResult().Result);
            //var info = proxy.GetSurveyProperties(4).GetAwaiter().GetResult();
            //WriteLine(JsonConvert.SerializeObject(info));
            //info.Active = "N";
            //WriteLine(proxy.SetSurveyProperties(4, info).GetAwaiter().GetResult().Result);
            //WriteLine(JsonConvert.SerializeObject(proxy.GetSurveyProperties(4).GetAwaiter().GetResult()));
            //WriteLine(proxy.ActivateSurvey(4).GetAwaiter().GetResult().Result);
            //WriteLine(proxy.DeleteSurvey(2).GetAwaiter().GetResult().Result);
            //WriteLine(proxy.AddSurvey(4, "hello earthians", "en").GetAwaiter().GetResult().Result);
            //WriteLine(proxy.GetSummary(4).GetAwaiter().GetResult().CompletedResponses);
            //WriteLine(proxy.ImportSurvey(2, "test10", File.ReadAllText("../../../Limesurvey_sample_survey_Assessment.lss"), "lss").GetAwaiter().GetResult().Result);
            ReadKey();
        }
    }
}
