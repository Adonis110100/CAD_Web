using CADWeb.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CADWeb.SQL
{
    public class SQLQuery
    {
        SqlConnection conn = SQLConnect.GetConnection();

        /// <summary>
        /// 查询数据库判断用户是否存在，同时返回用户状态，以中杠符分隔"|"
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public string IsUserExist(string userName, string userPassword, string school, out string newUserName, out string gradeClass)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from userInfo where userName=@userName", conn);
            cmd.Parameters.AddWithValue("@userName", userName);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (userPassword.Equals(reader["userPassword"]))
                    {
                        string userState = reader["userState"].ToString();
                        gradeClass = "";
                        newUserName = userName;
                        if (reader["school"] != DBNull.Value && !reader["school"].ToString().Equals(school))
                        {
                            conn.Close();

                            return "false" + "|-1";
                        }
                        reader.Close();
                        conn.Close();
                        return "true" + "|" + userState;
                    }
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            cmd = new SqlCommand("select 年级班级 from 学校班级信息 where 学校名=@school", conn);
            cmd.Parameters.AddWithValue("@school", school);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                List<string> list = new List<string>();
                //cmd.Dispose();
                while (reader.Read())
                {
                    list.Add(reader["年级班级"].ToString());
                }
                reader.Close();
                foreach (string x in list)
                {
                    string tableName = "classInfo_" + x;
                    cmd = new SqlCommand("use [CAD__" + school + "] " +
                        "select * from " + tableName + " where 学号=@userName", conn);
                    cmd.Parameters.AddWithValue("@userName", userName);
                    //cmd.Parameters.AddWithValue("@school", school);
                    //cmd.Parameters.AddWithValue("@tableName", tableName);
                    SqlDataAdapter sdap = new SqlDataAdapter();
                    sdap.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    sdap.Fill(dt);
                    if (dt == null)
                        continue;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (userPassword.Equals(dr["密码"]))
                        {
                            string userState = dr["学生状态"].ToString();
                            newUserName = dr["姓名"].ToString();
                            gradeClass = x;
                            return "true" + "|" + userState;
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            newUserName = userName;
            gradeClass = "";
            return "false" + "|-1";
        }

        public string QuerySchoolName()
        {
            string schools = "";
            conn.Open();
            string sql = "select name from sysdatabases where [name] like 'CAD__%' and [name] not like 'CAD-%'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    schools += reader["name"].ToString().Substring(5) + "|";
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return schools;
        }

        internal void SchoolManage(string name, string queryType, string type)
        {
            conn.Open();
            string sql = "";
            if (queryType.Equals("Add"))
                sql = "insert into 学校信息 (学校名,学校冻结状态)values(@name,1) " +
                "if exists (select * from sys.databases where name = 'CAD__" + name + "')" +
                "return " +
                "create database [CAD__" + name + "]";
            if (queryType.Equals("Delete"))
                sql = "delete  from 学校信息 where 学校名=@name " +
                    "drop database [CAD__" + name + "] ";
            if (queryType.Equals("Block"))
                sql = "update 学校信息 set 学校冻结状态=0 where 学校名=@name";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 根据数据库查询用户类型
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string QueryUserType(string userName)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand cmd = new SqlCommand("select userType from userInfo where userName=@temp", conn);
            cmd.Parameters.AddWithValue("@temp", userName);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return reader["userType"].ToString();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return "Student";
        }

        public IList UserQuery(int queryNum, string school)
        {
            IList queryList = new List<UserInfo>();
            switch (queryNum)
            {
                case 1:
                    QuerySchool(out queryList);
                    break;
                case 2:
                    QueryUserInfo("Admin", out queryList, school);
                    break;
                case 3:
                    QueryUserInfo("Teacher", out queryList, school);
                    break;
                case 4:
                    QueryUserInfo("Student", out queryList, school);
                    break;
                case 5:
                    QueryQuestionName(out queryList);
                    break;
                case 6:
                    QueryTestScore(out queryList, school);
                    break;
            }
            return queryList;
        }

        private void QuerySchool(out IList queryList)
        {
            queryList = new List<UserInfo>();
            conn.Open();
            string sql = "select 学校名,学校冻结状态 from 学校信息 ";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo userInfo = new UserInfo
                    {
                        School = reader["学校名"].ToString(),
                        UserState = Convert.ToInt32(reader["学校冻结状态"].ToString())
                    };
                    queryList.Add(userInfo);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        private void QueryUserInfo(string userType, out IList queryList, string school)
        {
            queryList = new List<UserInfo>();
            conn.Open();
            string sql;
            if (userType.Equals("Admin"))
                sql = "select * from userInfo where userType='" + userType + "'";
            else if (userType.Equals("Teacher"))
                sql = "select * from userInfo where userType='" + userType + "'";
            else
            {
                sql = "select 年级班级 from 学校班级信息 where 学校名='" + school + "'";//"use [CAD__" + school + "] select * from classInfo_1701";
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                try
                {
                    SqlDataReader reader = cmd1.ExecuteReader();
                    List<string> list = new List<string>();//学校所有班级列表
                    while (reader.Read())
                    {
                        list.Add(reader["年级班级"].ToString());
                    }
                    reader.Close();
                    //sql = "use [CAD__" + school + "] select * from ";
                    foreach (string temp in list)
                    {
                        //sql=sql+"classInfo_" + list[i];
                        //if (i != list.Count - 1)
                        //    sql = sql + ",";
                        UserInfo userInfo = new UserInfo
                        {
                            UserName = "{" + temp + "}",
                        };
                        queryList.Add(userInfo);
                        sql = "use [CAD__" + school + "] select * from classInfo_" + temp + "";
                        cmd1 = new SqlCommand(sql, conn);
                        reader = cmd1.ExecuteReader();
                        while (reader.Read())
                        {
                            userInfo = new UserInfo
                            {
                                UserPassword = reader["学号"].ToString(),
                                UserName = reader["姓名"].ToString(),
                                UserState = Convert.ToInt32(reader["学生状态"]),
                                GradeClass = temp,
                            };
                            queryList.Add(userInfo);
                        }
                        reader.Close();
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    conn.Close();
                }
                return;
            }

            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo userInfo = new UserInfo
                    {
                        UserName = reader["userName"].ToString(),
                        UserPassword = reader["UserPassword"].ToString(),
                        UserType = userType,
                        UserState = Convert.ToInt32(reader["UserState"])
                    };
                    queryList.Add(userInfo);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        private void QueryQuestionName(out IList queryList)
        {
            queryList = new List<string>();
            conn.Open();
            try
            {
                queryList.Add("<选择题>");
                string sql = "select 题目,题目状态 from 选择题库";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    queryList.Add(reader["题目"] + "|" + reader["题目状态"]);
                }
                reader.Close();
                queryList.Add("<判断题>");
                sql = "select 题目,题目状态 from 判断题库";
                cmd = new SqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    queryList.Add(reader["题目"] + "|" + reader["题目状态"]);
                }
                reader.Close();
                queryList.Add("<作图题>");
                sql = "select 题目,题目状态 from 作图题库";
                cmd = new SqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    queryList.Add(reader["题目"] + "|" + reader["题目状态"]);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                conn.Close();
            }
        }

        private void QueryTestScore(out IList queryList, string school)
        {
            queryList = new List<string>();
            conn.Open();
            try
            {
                List<string> scoreTableName = new List<string>();
                SqlCommand cmd = new SqlCommand("use [CAD__" + school + "] SELECT Name FROM SysObjects Where XType='U'  and name like 'testScore_%'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    scoreTableName.Add(reader["name"].ToString());
                }
                reader.Close();
                foreach (string temp in scoreTableName)
                {
                    queryList.Add("{" + temp.Remove(0, 10) + "}");
                    string sql = "select * from " + temp + "";
                    cmd = new SqlCommand(sql, conn);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        queryList.Add(reader["学号"] + "|" + reader["姓名"] + "|" + reader["年级班级"] + "|" + reader["得分"]);
                    }
                    reader.Close();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<UserInfo> QueryClassInfo(string schoolName)
        {
            List<UserInfo> queryList = new List<UserInfo>();
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("select * from 学校班级信息 where 学校名='" + schoolName + "'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo userInfo = new UserInfo
                    {
                        UserName = reader["年级班级"].ToString(),
                        UserState = Convert.ToInt32(reader["冻结状态"])
                    };
                    queryList.Add(userInfo);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return queryList;
        }

        public void DeleteTestScoreData(string stuNum, string sql)
        {
            ExecuteSqlNonQuery(stuNum, sql);
        }

        public void SuperAdminOperation(string name, string operation, string type)
        {
            switch (operation)
            {
                case "Add":
                    ExecuteSqlNonQuery(name, "insert into userInfo(userName, userPassword, userType, userState) values(@name, '88888888', 'Admin', '1')");
                    break;
                case "Block":
                    ExecuteSqlNonQuery(name, "update userInfo set userState=0 where userName=@name");
                    break;
                case "Delete":
                    switch (type)
                    {
                        case "学生":
                            type = "Student";
                            ExecuteSqlNonQuery(name, "delete from userInfo where userName=@name and userType='" + type + "'");
                            break;
                        case "教师":
                            type = "Teacher";
                            ExecuteSqlNonQuery(name, "delete from userInfo where userName=@name and userType='" + type + "'");
                            break;
                        case "管理员":
                            type = "Admin";
                            ExecuteSqlNonQuery(name, "delete from userInfo where userName=@name and userType='" + type + "'");
                            break;
                        case "题库":
                            ExecuteSqlNonQuery(name, "delete from 选择题库 where 题目=@name");
                            ExecuteSqlNonQuery(name, "delete from 判断题库 where 题目=@name");
                            ExecuteSqlNonQuery(name, "delete from 作图题库 where 题目=@name");
                            break;
                    }
                    break;
            }
        }

        public void SuperAdminBatchDelete(string[] strArray, string type)
        {
            string sql = "";
            if (type.Equals("学生"))
            {
                sql = "delete from userInfo where userName in(";
            }
            else if (type.Equals("题库"))
            {
                sql = "delete from 选择题库 where 题目 in(";
            }
            foreach (string temp in strArray)
            {
                sql += "'" + temp + "',";
            }
            sql = sql.TrimEnd(',') + ")";
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(sql.Replace("选择题库", "判断题库"), conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(sql.Replace("选择题库", "判断题库").Replace("判断题库", "作图题库"), conn);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void AdminOpertion(string name, string operation, string userType)
        {
            switch (operation)
            {
                case "Add":
                    switch (userType)
                    {
                        case "教师":
                            ExecuteSqlNonQuery(name, "insert into userInfo(userName, userPassword, userType, userState) values(@name, '654321', 'Teacher', 1)");
                            break;
                        case "班级":
                            string[] names = name.Split('|');
                            string className = names[0];
                            string schoolName = names[1];
                            ExecuteSqlNonQuery(className, "insert into 学校班级信息(学校名, 年级班级, 冻结状态) values('" + schoolName + "', @name, 1)");
                            break;
                        case "学生":
                            ExecuteSqlNonQuery(name, "insert into userInfo(userName, userPassword, userType, userState) values(@name, '123456', 'Student', 1)");
                            break;
                    }
                    break;
                case "0": case "1":
                    switch (userType)
                    {
                        case "教师":
                            ExecuteSqlNonQuery(name, "update userInfo set userState=" + operation + " where userName=@name and userType='Teacher'");
                            break;
                        case "班级":
                            string[] names = name.Split('|');
                            string className = names[0];
                            string schoolName = names[1];
                            ExecuteSqlNonQuery(className, "update 学校班级信息 set 冻结状态=" + operation + " where 学校名='" + schoolName + "' and 年级班级=@name");
                            break;
                        case "学生":
                            string[] Snames = name.Split('|');//"16240003|深职院|{1701}"
                            string stuNum = Snames[0];
                            string SschoolName = Snames[1];
                            string SclassName = Snames[2].TrimEnd('}').TrimStart('{');
                            ExecuteSqlNonQuery(stuNum, "use [CAD__" + SschoolName + "] update classInfo_" + SclassName + " set 学生状态=" + operation + " where 学号=@name");
                            break;
                    }
                    break;
            }
        }

        public string QueryTableNameFromStuName(string name)
        {
            string str = "";
            conn.Open();
            SqlCommand cmd = new SqlCommand("DECLARE @return_value int EXEC @return_value = [dbo].[P_SYSTEM_FindData] @value = N'" + name + "' SELECT  'Return Value' = @return_value", conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    str = reader["tablename"].ToString();
                    if (!str.Contains("classInfo_"))
                    {
                        continue;
                    }
                    break;
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return str;
        }

        public Dictionary<string, string> QueryAllExamState()
        {
            Dictionary<string, string> examDic = new Dictionary<string, string>();
            conn.Open();
            SqlCommand cmd = new SqlCommand("select 卷名,试卷状态,答题时长 from 题组库", conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    examDic.Add(reader["卷名"].ToString() + "|" + reader["答题时长"].ToString(), reader["试卷状态"].ToString());
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return examDic;
        }

        public List<ExamInfo> QueryExamsInfo()
        {
            List<ExamInfo> exams = new List<ExamInfo>();
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from 题组库", conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ExamInfo examInfo = new ExamInfo
                    {
                        ExamName = reader["卷名"].ToString(),
                        ExamType = reader["试卷类型"].ToString(),
                        ExamState = reader["试卷状态"].ToString().TrimEnd('|')
                    };
                    exams.Add(examInfo);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return exams;
        }

        public ExamInfo QueryExamInfoByName(string examName)
        {
            ExamInfo examInfo = new ExamInfo();
            string CQnumbers = "";
            string JQnumbers = "";
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from 题组库 where 卷名='" + examName + "'", conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    examInfo.ExamName = examName;
                    examInfo.ExamType = reader["试卷类型"].ToString();
                    examInfo.Score = Convert.ToInt32(reader["总分"] == DBNull.Value ? 0 : reader["总分"]);

                    CQnumbers = reader["单选题题目序号"].ToString();
                    JQnumbers = reader["判断题题目序号"].ToString();
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            examInfo.ChoiceQInfos = QueryChoiceQInfoByExamInfo(CQnumbers);
            examInfo.JudgementQInfos = QueryJudgementQInfoByExamInfo(JQnumbers);
            return examInfo;
        }

        private List<ChoiceQInfo> QueryChoiceQInfoByExamInfo(string numbers)
        {
            List<ChoiceQInfo> infos = new List<ChoiceQInfo>();
            string[] num = numbers.Split(',');
            string sql = "select * from 选择题库 where ";
            foreach (string temp in num)
            {
                sql += "xid='" + temp + "' or ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ChoiceQInfo info = new ChoiceQInfo
                    {
                        ChoiceQNum = Convert.ToInt32(reader["xid"]),
                        QuestionInfo = reader["题目"].ToString(),
                        Picture_Src = reader["图片路径"].ToString(),
                        ChoiceA = reader["选项A"].ToString(),
                        ChoiceB = reader["选项B"].ToString(),
                        ChoiceC = reader["选项C"].ToString(),
                        ChoiceD = reader["选项D"].ToString(),
                        AnswerIs = reader["答案"].ToString(),
                        AnswerInfo = reader["答案解析"].ToString(),
                        State = Convert.ToInt32(reader["题目状态"])
                    };
                    infos.Add(info);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            return infos;
        }

        private List<JudgementQInfo> QueryJudgementQInfoByExamInfo(string numbers)
        {
            List<JudgementQInfo> infos = new List<JudgementQInfo>();
            string[] num = numbers.Split(',');
            string sql = "select * from 判断题库 where ";
            foreach (string temp in num)
            {
                sql += "pid='" + temp + "' or ";
            }
            sql = sql.Substring(0, sql.Length - 3);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    JudgementQInfo info = new JudgementQInfo
                    {
                        JudgementQNum = Convert.ToInt32(reader["pid"]),
                        QuestionInfo = reader["题目"].ToString(),
                        Picture_Src = reader["图片路径"].ToString(),
                        AnswerIs = reader["答案"].ToString(),
                        AnswerInfo = reader["答案解析"].ToString(),
                        State = Convert.ToInt32(reader["题目状态"])
                    };
                    infos.Add(info);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return infos;
        }

        public string QueryStuInfoFromClass(string school, string className, string stuName)
        {
            string stuInfo = "";
            conn.Open();
            string sql = "use [CAD__" + school + "] select * from classInfo_" + className + " where 姓名=@stuName";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@stuName", stuName);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stuInfo = reader["学号"].ToString() + "|" + stuName + "|" + className + "|" + reader["答题状态"].ToString() + "|" + school;
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return stuInfo;
        }

        public void InsertStudentTestScore(string school, string examName, string num, string stuName, string className, string choiceQAnswer, string judgementQAnswer, long time, int count, string drawingQAnswer = null)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("use [CAD__" + school + "] " +
                "if EXISTS(select * from testScore_" + examName + " where 姓名='" + stuName + "')" +
                "  update testScore_" + examName + " set 学号='" + num + "',年级班级='" + className + "',单选题答案='" + choiceQAnswer + "',判断题答案='" + judgementQAnswer + "',答题时间='" + time + "',答题数='" + count + "'" +
                "else " +
                "   insert into testScore_" + examName + "(学号,姓名,年级班级,单选题答案,判断题答案,答题时间,答题数) " +
                "   values('" + num + "', '" + stuName + "', '" + className + "', '" + choiceQAnswer + "', '" + judgementQAnswer + "', '" + time + "', '" + count + "')", conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 答案和分数获取
        /// </summary>
        /// <param name="school"></param>
        /// <param name="examName"></param>
        /// <param name="stuName"></param>
        /// <returns></returns>
        public string QueryStudentTestAnswer(string school, string examName, string stuName)
        {
            string testInfo = String.Empty;
            conn.Open();
            string sql = "use [CAD__" + school + "] select * from testScore_" + examName + " where 姓名=@stuName";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@stuName", stuName);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int score = Convert.ToInt32(reader["得分"]);
                    if (score == -1)
                    {
                        return testInfo;
                    }
                    string img1 = "", img2 = "", img3 = "";
                    byte[] question;
                    if (reader["画图题答案图片1"] != DBNull.Value)
                    {
                        question = (byte[])reader["画图题答案图片1"];
                        img1 = Convert.ToBase64String(question);
                    }
                    if (reader["画图题答案图片1"] != DBNull.Value)
                    {
                        question = (byte[])reader["画图题答案图片2"];
                        img2 = Convert.ToBase64String(question);
                    }
                    if (reader["画图题答案图片1"] != DBNull.Value)
                    {
                        question = (byte[])reader["画图题答案图片3"];
                        img3 = Convert.ToBase64String(question);
                    }
                    testInfo = reader["单选题答案"].ToString() + "|" + reader["判断题答案"].ToString() + "|" + img1.ToString() + "|" + img2.ToString() + "|" + img3.ToString() + "|" + score;
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return testInfo;
        }

        public void UpdateAnswerStateInClass(string school, string className, string examName, string stuName)
        {
            ExecuteSqlNonQuery(stuName, "use [CAD__" + school + "] update classInfo_" + className + " set 答题状态='" + examName + "&' where 姓名=@name");
        }

        public void UpdateQuestionState(string questionName, int updateState, string questionType)
        {
            ExecuteSqlNonQuery(questionName, "update " + questionType + " set 题目状态=" + updateState + " where 题目=@name");
        }

        public void UpdateExamState(string examName, string className)
        {
            ExecuteSqlNonQuery(examName, "update 题组库 set 试卷状态='" + className + "' where 卷名=@name");
        }

        private void ExecuteSqlNonQuery(string name, string sql)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}