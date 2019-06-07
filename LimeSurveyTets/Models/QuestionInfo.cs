using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LimeSurveyTest.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class QuestionInfo
    {
        public int Qid { get; set; }
        public int ParentQid { get; set; }
        public int Sid { get; set; }
        public int Gid { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Preg { get; set; }
        public string Help { get; set; }
        public string Other { get; set; }
        public string Mandatory { get; set; }
        public string QuestionOrder { get; set; }
        public string Language { get; set; }
        public string ScaleId { get; set; }
        public string SameDefault { get; set; }
        public string Relevance { get; set; }
        public string Modulename { get; set; }
        public string[] AvailableAnswers { get; set; }
        public Subquestion[] Subquestions { get; set; }
        public string Attributes { get; set; }
        public string AttributesLang { get; set; }
        public string Answeroptions { get; set; }
        public object Defaultvalue { get; set; }
    }

    public class Subquestion
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string ScaleId { get; set; }
    }
}
