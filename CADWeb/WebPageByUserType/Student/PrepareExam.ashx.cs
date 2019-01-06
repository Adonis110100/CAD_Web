using CADWeb.Model;
using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Student
{
    /// <summary>
    /// PrepareExam 的摘要说明
    /// </summary>
    public class PrepareExam : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "text/html";
            SQLQuery query = new SQLQuery();
            string examName = request["name"].ToString();
            ExamInfo examInfo = query.QueryExamInfoByName(examName);
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            int i = 0;
            int j = 0;
            int questionIndex = 0;

            //选择题处理
            List<ChoiceQInfo> choiceQInfos = examInfo.ChoiceQInfos;
            string htmlC = File.ReadAllText(baseDirectory + @"WebPageByUserType\Student\ExamQuestions\ChoiceQuestions.html");
            string contentC = "";
            for (; i < choiceQInfos.Count; i++)
            {
                questionIndex++;
                contentC += string.Format(
                    "<div>" +
                        "<div>{0}、 {1}" +
                            "<div name='QuestionPic' style='display: none'>{2}</div>" +
                        "</div>" +
                        "<div><label for='ChoiceA{9}'><input type='radio' id='ChoiceA{9}' name='Choice{9}' /> A、 {3}</label></div>" +
                        "<div><label for='ChoiceB{9}'><input type='radio' id='ChoiceB{9}' name='Choice{9}' /> B、 {4}</label></div>" +
                        "<div><label for='ChoiceC{9}'><input type='radio' id='ChoiceC{9}' name='Choice{9}' /> C、 {5}</label></div>" +
                        "<div><label for='ChoiceD{9}'><input type='radio' id='ChoiceD{9}' name='Choice{9}' /> D、 {6}</label></div>" +
                        "<div name='Answer' style='display: none'>" +
                            "<div>正确答案： {7}</div>" +
                            "<div>解析： {8}</div>" +
                        "</div>" +
                    "</div>",
                    questionIndex, choiceQInfos[i].QuestionInfo, choiceQInfos[i].Picture_Src, choiceQInfos[i].ChoiceA, choiceQInfos[i].ChoiceB,
                    choiceQInfos[i].ChoiceC, choiceQInfos[i].ChoiceD, choiceQInfos[i].AnswerIs, choiceQInfos[i].AnswerInfo, i);
            }
            htmlC = htmlC.Replace("$Templet", contentC);
            string pathC = baseDirectory + @"WebPageByUserType\Student\ExamQuestions\" + examInfo.ExamName + "CQuestions.html";
            if (!File.Exists(pathC))
            {
                FileStream fs = new FileStream(pathC, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(htmlC);
                sw.Close();
            }

            //判断题处理
            List<JudgementQInfo> judgementQInfos = examInfo.JudgementQInfos;
            string htmlJ = File.ReadAllText(baseDirectory + @"WebPageByUserType\Student\ExamQuestions\JudgmentQuestions.html");
            string contentJ = "";
            for (; j < judgementQInfos.Count; j++)
            {
                questionIndex++;
                contentJ += string.Format(
                    "<div>" +
                        "<div>{0}、 {1}" +
                            "<div name='QuestionPic' style='display: none'>{2} </div>" +
                        "</div>" +
                        "<div><label for='ChoiceTrue{5}'><input type='radio' id='ChoiceTrue{5}' name='Choice{5}' /> 正确</label></div>" +
                        "<div><label for='ChoiceFalse{5}'><input type='radio' id='ChoiceFalse{5}' name='Choice{5}' /> 错误</label></div>" +
                        "<div name='Answer' style='display: none'>" +
                            "<div>正确答案： {3}</div><div>解析： {4}</div>" +
                        "</div>" +
                    "</div>",
                    questionIndex, judgementQInfos[j].QuestionInfo, judgementQInfos[j].Picture_Src, judgementQInfos[j].AnswerIs, judgementQInfos[j].AnswerInfo, j);
            }
            htmlJ = htmlJ.Replace("$Templet", contentJ);
            string pathJ = baseDirectory + @"WebPageByUserType\Student\ExamQuestions\" + examInfo.ExamName + "JQuestions.html";
            if (!File.Exists(pathJ))
            {
                FileStream fs = new FileStream(pathJ , FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(htmlJ);
                sw.Close();
            }

            response.Write(examInfo.ExamName + "|" + i + "+" + j);

        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}