using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace LimeSurveyTest.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ParticipantCredentials
    {
        public int Tid { get; set; }
        public string Token { get; set; }
        public ParticipantInfo ParticipantInfo { get; set; }
    }

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ParticipantInfo
    {
        public string Usesleft { get; set; }
        public string Completed { get; set; }
        public int? Tid { get; set; }
        public int? ParticipantId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Emailstatus { get; set; }
        public string Token { get; set; }
        public string Language { get; set; }
        public string Blacklisted { get; set; }
        public string Sent { get; set; }
        public string Remindersent { get; set; }
        public string Remindercount { get; set; }
        public DateTime? Validfrom { get; set; }
        public DateTime? Validuntil { get; set; }
        public int Mpid { get; set; }
    }
}
