using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class SurveySummary
    {
        public string CompletedResponses { get; set; }
        public string IncompleteResponses { get; set; }
        public string FullResponses { get; set; }
    }
}
