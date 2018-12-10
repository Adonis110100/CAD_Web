using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.Model
{
    public class ChoiceQInfo
    {
        public int ChoiceQNum { get; set; }
        public string QuestionInfo { get; set; }
        public string Picture_Src { get; set; }
        public string ChoiceA { get; set; }
        public string ChoiceB { get; set; }
        public string ChoiceC { get; set; }
        public string ChoiceD { get; set; }
        public string AnswerIs { get; set; }
        public string AnswerInfo { set; get; }
        public int State { get; set; }
    }
}