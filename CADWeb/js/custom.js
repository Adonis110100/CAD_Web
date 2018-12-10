

function getClassScoreTable(obj) {
   
    var ddl = obj.getElementById("DropDownList1")
    var index = ddl.selectedIndex;

    var Value = ddl.options[index].value;
    var Text = ddl.options[index].text;
    document.cookie = "gradeClassName=" + Text
    sessionStorage.setItem["class"]=Text

}

function setCookie(name, value) {
    var exp = new Date();
    exp.setTime(exp.getTime() + 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function getCookie(name) {
    var regExp = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    var arr = document.cookie.match(regExp);
    if (arr === null) {
        return null;
    }
    return unescape(arr[2]);
}

function BeginExam() {
    var exams = document.getElementsByName("exam");
    var examName = "";
    var examTime = "";
    var ischeaked = false;
    for (var i = 0; i < exams.length; i++) {
        if (exams[i].checked) {
            examName = exams[i].id.substring(5);
            examTime = document.getElementById("time_" + examName).innerHTML;
            ischeaked = true;
            break;
        }
    }
    if (!ischeaked) {
        alert("请先选择你要参加的考试！");
        return;
    }
    if (examTime == "不限时长") {
        examTime = "-1";
    } else {
        examTime = examTime.replace("分钟", "");
    }
    sessionStorage.setItem("examTime", examTime);
    //sessionStorage.setItem("examName", examName);
    $.ajax({
        type: "Post",
        url: "PrepareExam.ashx",
        dataType: "text",
        data: { name: examName },
        success: function (data) {
            var str = data.split('|');
            $.cookie('examName', str[0]);
            $.cookie('examQSum', str[1]);
            alert("请开始答题！");
            window.location.href = "Exam.html";
        },
        error: function () {
            alert("未知的错误发生了！请联系管理员处理！");
            window.location.href = "Student.html";
        }
    });
}

function BeginQueryAnswer(temp) {
    var allAnswerArray = temp.split('&');
    var radios = document.getElementsByName("exam");
    var answer = "";
    var ischeaked = false;
    for (var i = 0; i < radios.length; i++) {
        if (radios[i].checked) {
            answer = allAnswerArray[i];
            sessionStorage.setItem("answer_examName", radios[i].id.substring(5));
            ischeaked = true;
            break;
        }
    }
    if (!ischeaked) {
        alert("请先选择你要查询的试题！");
        return;
    }
    sessionStorage.setItem("stuAnswer", answer);
    window.location.href = "ExamResult.html";
}

function BlockOrUnblock(obj, type) {
    var index = obj.name;
    if (document.getElementById("state" + index).innerHTML == type) {
        alert("您选择的题目是" + type + "状态，请重新选择！");
        return;
    }
    var question = document.getElementById("question" + index);
    var questionName = question.innerHTML;
    var questionType = document.getElementById("type" + question.getAttribute("name")).innerHTML;
    $.ajax({
        type: "Post",
        url: "AdminQueryQuestion.ashx",
        dataType: "text",
        data: { questionName: questionName, operationType: type, questionType: questionType },
        success: function (data) {
            if (data == 1) {
                alert("解冻成功！");
            }else if (data == 0) {
                alert("冻结成功！");
            }
            window.location.href = "";
        },
        error: function () {
            alert("未知的错误发生了！请联系管理员处理！");
            return;
        }
    });
}

function updateTestState(obj) {

    let len = document.getElementsByName("exam").length;
    let selectid = "";  //  selectvalue为radio中选中的值
    for (let i = 0; i < len; i++) {
        if (document.getElementsByName("exam")[i].checked) {
            selectid = document.getElementsByName("exam")[i].id;
        }
    }
    //sessionStorage.setItem("testName",obj.getElementById("exam_" + selectid).Value)
    //sessionStorage.setItem("classString", obj.getElementById("examState_" + selectid).Value)
    let testName = document.getElementById("exam_" + selectid).innerHTML
    let classString = document.getElementById("examState_" + selectid).innerHTML
    let addClassString = document.getElementById("inputClass").value
    //alert("fuck");
    window.location.href = 'updateClassTestState.ashx?testName=' + testName + '&classString=' + classString + '&addClassString=' + addClassString;
}