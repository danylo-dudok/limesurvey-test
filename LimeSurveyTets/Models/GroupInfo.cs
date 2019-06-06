using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest.Models
{
    public class GroupInfo
    {
        public int Gid { get; set; }
        public int Sid { get; set; }
        public string GroupName { get; set; }
        public string GroupOrder { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string RandomizationGroup { get; set; }
        public string Grelevance { get; set; }
    }
}
