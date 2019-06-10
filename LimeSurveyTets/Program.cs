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

            var newSurveyId = 13;

            var token = proxy.ListParticipants(4, 0, 10).GetAwaiter().GetResult();
            Console.WriteLine(JsonConvert.SerializeObject(token));
            Console.WriteLine(JsonConvert.SerializeObject(proxy.GetResponseIds(4, token.First().Token).GetAwaiter().GetResult()));
            //var response = new ResponseInfo
            //{

            //};
            //Console.WriteLine(proxy.AddResponse(4, response).GetAwaiter().GetResult().Result);

            //WriteLine(JsonConvert.SerializeObject(proxy.ExportResponses(4).GetAwaiter().GetResult()));

            //var part = proxy.ListParticipants(12, 0, 10).GetAwaiter().GetResult().First();
            //part.ParticipantInfo.Lastname = "Ivanov";
            //WriteLine(JsonConvert.SerializeObject(proxy.GetParticipantProperties(12, 1).GetAwaiter().GetResult()));
            //WriteLine(proxy.GetResponseIds(newSurveyId).GetAwaiter().GetResult().Result);

            //WriteLine(JsonConvert.SerializeObject(proxy.ListParticipants(12, 0, 10).GetAwaiter().GetResult()));

            //WriteLine(proxy.AddSurvey(newSurveyId, "A new survey", "en").GetAwaiter().GetResult().Result.ToString());
            //var survey = proxy.GetSurveyProperties(newSurveyId).GetAwaiter().GetResult();
            //var lang = proxy.GetLanguageProperties(newSurveyId).GetAwaiter().GetResult();
            //survey.Showwelcome = "Hello not everyone";
            //lang.SurveylsDescription = "This survey is created.";
            //WriteLine(proxy.SetSurveyProperties(newSurveyId, survey).GetAwaiter().GetResult().Result.ToString());
            //WriteLine(proxy.SetLanguageProperties(newSurveyId, lang).GetAwaiter().GetResult().Result.ToString());
            //WriteLine(proxy.AddGroup(newSurveyId, "Survey group 2", "This group created for testing.").GetAwaiter().GetResult());
            //var groupId = proxy.ListGroups(newSurveyId).GetAwaiter().GetResult().First().Gid;
            //WriteLine(proxy.ImportQuestion(newSurveyId, groupId, File.ReadAllText("../../../limesurvey_question_5.lsq"), "lsq", "question1").GetAwaiter().GetResult().Result);
            //WriteLine(proxy.ActivateSurvey(newSurveyId).GetAwaiter().GetResult().Result);

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
            //WriteLine(proxy.GetSummary(4).GetAwaiter().GetResult().CompletedResponses);
            //WriteLine(proxy.ImportSurvey(2, "test10", File.ReadAllText("../../../Limesurvey_sample_survey_Assessment.lss"), "lss").GetAwaiter().GetResult().Result);
            ReadKey();
        }
    }
}
