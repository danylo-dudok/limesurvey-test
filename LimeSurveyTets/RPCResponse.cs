using System;
using System.Collections.Generic;
using System.Text;

namespace LimeSurveyTest
{
    public class RPCResponse
    {
        public int Id { set; get; }
        public object Result { set; get; }
        public string Status { get; set; }
        public string Error { set; get; }
    }
}
