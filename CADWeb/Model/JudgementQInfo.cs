using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.Model
{
    public class JudgementQInfo
    {
        public int JudgementQNum { get; set; }
        public string QuestionInfo { get; set; }
        public string Picture_Src { get; set; }
        public string AnswerIs { get; set; }
        public string AnswerInfo { set; get; }
        public int State { get; set; }
    }
}