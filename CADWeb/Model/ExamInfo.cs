using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.Model
{
    public class ExamInfo
    {
        public string ExamName { get; set; }
        public string ExamType { get; set; }
        public string ExamState { get; set; }

        public List<ChoiceQInfo> ChoiceQInfos { get; set; }
        public int ChoiceQCount { get { return ChoiceQInfos.Count; } }
        public List<JudgementQInfo> JudgementQInfos { get; set; }
        public int JudgementQCount { get { return JudgementQInfos.Count; } }

        public int TotalQCount { get { return ChoiceQCount + JudgementQCount; } }
        public int Score { get; set; }
    }
}