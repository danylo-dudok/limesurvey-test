﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest.Models
{
    public class ParticipantInfo
    {
        public int TId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Token { get; set; }
    }
}