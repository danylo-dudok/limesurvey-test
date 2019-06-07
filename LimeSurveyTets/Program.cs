using System;
using static System.Console;
using System.IO;
using Newtonsoft.Json;
using LimeSurveyTest.Models;
using System.Linq;

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

            var newSurveyId = 12;

            WriteLine(proxy.AddSurvey(newSurveyId, "Are you a human?", "en").GetAwaiter().GetResult().Result.ToString());
            var survey = proxy.GetSurveyProperties(newSurveyId).GetAwaiter().GetResult();
            var lang = proxy.GetLanguageProperties(newSurveyId).GetAwaiter().GetResult();
            survey.Showwelcome = "Hello everyone";
            lang.SurveylsDescription = "This survey is created to determine if you are a human or not.";
            WriteLine(proxy.SetSurveyProperties(newSurveyId, survey).GetAwaiter().GetResult().Result.ToString());
            WriteLine(proxy.SetLanguageProperties(newSurveyId, lang).GetAwaiter().GetResult().Result.ToString());
            WriteLine(proxy.AddGroup(newSurveyId, "Some survey group", "This group created only in testings purposes.").GetAwaiter().GetResult());
            var groupId = proxy.ListGroups(newSurveyId).GetAwaiter().GetResult().First().Gid;
            WriteLine(proxy.ImportQuestion(newSurveyId, groupId, File.ReadAllText("../../../limesurvey_question_5.lsq"), "lsq", "hello").GetAwaiter().GetResult().Result);
            WriteLine(proxy.ActivateSurvey(newSurveyId).GetAwaiter().GetResult().Result);

            // ../../../limesurvey_question_5.lsq


            //WriteLine(JsonConvert.SerializeObject(proxy.ListSurveys().GetAwaiter().GetResult()));

            //WriteLine(proxy.ListQuestions(4, 3).GetAwaiter().GetResult().First().Question);
            //WriteLine(proxy.ListGroups(4).GetAwaiter().GetResult().First().Gid);

            //WriteLine(proxy.GetGroupProperties())

            //var lang = proxy.GetLanguageProperties(4).GetAwaiter().GetResult();
            //lang.SurveylsDescription = "This is my description to api";
            //WriteLine(proxy.SetLanguageProperties(4, lang).GetAwaiter().GetResult().Result);

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
