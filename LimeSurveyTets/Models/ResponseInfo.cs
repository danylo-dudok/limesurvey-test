using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest.Models
{
    public class ResponseInfo
    {
        public string Id { get; set; }
        public string Submitdate { get; set; }
        public string Lastpage { get; set; }
        public string Startlanguage { get; set; }
        public string Seed { get; set; }
        public string Startdate { get; set; }
        public string Datestamp { get; set; }
        public string Ipaddr { get; set; }
        public IEnumerable<string> Questions { get; set; }
    }
}
