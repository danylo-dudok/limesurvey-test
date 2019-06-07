using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class LanguageInfo
    {
        public string SurveylsSurveyId { get; set; }
        public string SurveylsLanguage { get; set; }
        public string SurveylsTitle { get; set; }
        public string SurveylsDescription { get; set; }
        public string SurveylsWelcometext { get; set; }
        public string SurveylsEndtext { get; set; }
        public object SurveylsPolicyNotice { get; set; }
        public object SurveylsPolicyError { get; set; }
        public object SurveylsPolicyNoticeLabel { get; set; }
        public string SurveylsUrl { get; set; }
        public string SurveylsUrldescription { get; set; }
        public string SurveylsEmailInviteSubj { get; set; }
        public string SurveylsEmailInvite { get; set; }
        public string SurveylsEmailRemindSubj { get; set; }
        public string SurveylsEmailRemind { get; set; }
        public string SurveylsEmailRegisterSubj { get; set; }
        public string SurveylsEmailRegister { get; set; }
        public string SurveylsEmailConfirmSubj { get; set; }
        public string SurveylsEmailConfirm { get; set; }
        public string SurveylsDateformat { get; set; }
        public object SurveylsAttributecaptions { get; set; }
        public string EmailAdminNotificationSubj { get; set; }
        public string EmailAdminNotification { get; set; }
        public string EmailAdminResponsesSubj { get; set; }
        public string EmailAdminResponses { get; set; }
        public string SurveylsNumberformat { get; set; }
        public object Attachments { get; set; }
    }
}
