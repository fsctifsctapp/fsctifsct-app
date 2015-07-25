var Gcoop = new Object();

Gcoop.ProgressUrl = "";
Gcoop.ProgressName = "";
Gcoop.ExtraFunction = null;
Gcoop.DisplayProgressCloseButton = false;

Gcoop.CheckDw = function(sender, row, column, columnName, checkValue, uncheckValue){
    if(column == columnName){
        if(!Gcoop.IsCheckSupport()){
            var vv = sender.GetItem(row, columnName);
            if(vv == checkValue){
                sender.SetItem(row, columnName, uncheckValue);
            } else {
                sender.SetItem(row, columnName, checkValue);
            }
            sender.AcceptText();
        }
    }
}

Gcoop.Explode = function(bitVar, fullVar){
    var v = new Array();
    var sss;
    var a = 0;
    var i = 0;
    var ii = 0;
    var lengthIndex = bitVar.length;
    sss = fullVar.replace(/^\s+|\s+$/g, ''); // trim();
    while (true) {
        if (ii > 0) {
            i = ii + (lengthIndex - 1);
        }
        ii = sss.indexOf(bitVar, ii);
        if (ii < 0) {
            v[a] = sss.substr(i);
        } else {
            v[a] = sss.substring(i, ii);
        }
        if (ii < 0) {
            ii = -1;
        } else {
            ii++;
        }
        if (ii < 0) {
            break;
        }
        a++;
    }
    return v;
}

Gcoop.Focus = function(elName){
    var realElementName = (elName == null || elName == undefined) ? Gcoop.GetLastFocus() : elName;
    document.forms["aspnetForm"].elements[realElementName].focus();
}

Gcoop.GetApplication = function(){
    return Gcoop.Application;
}

Gcoop.GetBrowser = function(){
    if("Netscape" != navigator.appName){
        return navigator.appName.indexOf("Explorer") > 0 ? "Internet Explorer" : navigator.appName;
    }
    if(navigator.userAgent.indexOf("Chrome") > 0){
        return "Chrome";
    } else if(navigator.userAgent.indexOf("Safari") > 0){
        return "Safari";
    } else if(navigator.userAgent.indexOf("Firefox") > 0){
        return "Firefox";
    } else if(navigator.userAgent.indexOf("BrowserNG") > 0){
        return "Nokia5800-BrowserNG";
    } else {
        return navigator.appName;
    }
}

Gcoop.GetEl = function (id){
    var webPageType = "";
    try{
        webPageType = document.getElementById("webPageType").value;
    } catch(Err){}
    if(webPageType == "w_dlg_xx_xxxxxx"){
        return document.getElementById(id);
    } else{
        try{
            var resultElement = document.getElementById("ctl00_ContentPlace_" + id);
            if(resultElement == null){
                resultElement = document.getElementById(id);
            }
            if(resultElement == null){
                return null;
            }
            return resultElement;
        }catch(err){
            return null;
        }
    }
}

Gcoop.GetElForm = function (name) {
    return document.aspnetForm.elements[name];
}

Gcoop.GetLastFocus = function(){
    return Gcoop.GetEl("tempLastFocus").value;
}

Gcoop.GetCurrentPage = function(){
    //return document.getElementById("ctl00_HCurrentPage").value;
    return Gcoop.CurrentPage;
}

Gcoop.GetUrl = function(){
    return Gcoop.Url;
}

Gcoop.IsCheckSupport = function(){
    return Gcoop.GetBrowser() == "Internet Explorer" || Gcoop.GetBrowser() == "Firefox";
}

Gcoop.IsNum = function(number){
    var num;
    try{
        number = number + "";
        if(number.substring(0, 1) == "-"){
            num = number.substr(1);
        } else {
            num = number;
        }
        var ch = num;
        var len, digit;
        if(ch == " "){
            return false;
            len=0;
        }else{
            len = ch.length;
        }
        for(var i=0 ; i<len ; i++)
        {
            digit = ch.charAt(i)
            if(digit >= "0" && digit <= "9"){
                //
            }else{
                return false;
            }
        }
        return true;
    }catch(Err){return false;}
}

Gcoop.IsValid = function(text){
    try{
        if(text != "undefined" && text != null && text != ""){
            return true;
        } else {
            return false;
        }
    }catch(err){ return false;}
}

Gcoop.OpenDlg = function(dlgwidth, dlgheight, pageaspx, sendargument){
    var left = (screen.width/2)-(dlgwidth/2);
    var top = (screen.height/2)-(dlgheight/2);
    top = (top-40) < 0 ? top : top - 40;
    var getUrl = Gcoop.GetUrl();// document.getElementById("ctl00_HUrl").value;
    var getApp = Gcoop.GetApplication();// document.getElementById("ctl00_HApplication").value;
    if(getUrl == null){
        alert("PageLoad not ready complete");
    }else{
        sendargument = sendargument == null ? "" : sendargument;
        var dlgurl = getUrl + "Applications/" + getApp + "/dlg/" + pageaspx + sendargument;
        window.open(dlgurl, null, "height="+dlgheight+",width="+dlgwidth+",top="+top+",left="+left+",status=0,toolbar=0,menubar=0,location=0,resizable=0");
    }
}

Gcoop.OpenIFrame = function(ifWidth, ifHeight, ifUrl, ifQueryString, extraUrl) {
    var ni = document.getElementById('iFrameMaster');
    var newdiv = document.createElement('div');
    var divIdName = 'iFrameChild';
    newdiv.setAttribute('id', divIdName);
    ni.appendChild(newdiv);
    
    var elNew = document.getElementById(divIdName);
    
    ifQueryString = ifQueryString == null || ifQueryString == undefined ? "" : Gcoop.Trim(ifQueryString);
    var newUrl = "";//Gcoop.GetUrl() + "Applications/" + Gcoop.GetApplication() + "/dlg/" +  ifUrl + ifQueryString;
    if(extraUrl != null && extraUrl != undefined){
        newUrl = extraUrl;
    } else {
        newUrl = Gcoop.GetUrl() + "Applications/" + Gcoop.GetApplication() + "/dlg/" +  ifUrl + ifQueryString;
    }
    
    elNew.style.top = 0; 
    elNew.style.left = 0;
    elNew.style.width = (screen.width - 20) + "px";
    elNew.style.height = "2500px";
    elNew.style.zIndex  = 999;
    elNew.style.position = "absolute";
    elNew.style.backgroundImage = "url('"+ Gcoop.GetUrl() +"img/tranparent.png')";
    elNew.innerHTML = "<br /><br /><br /><br /><br /><br /><div align='center'><iframe style='background-color:white;' src='"+ newUrl +"' width='"+ ifWidth +"' height='"+ ifHeight +"' frameborder='0'></iframe></div>";
}

Gcoop.OpenPopup = function(url){
    window.open(url);
}

Gcoop.OpenProgressBar = function(progressName, warnning, showCloseButton, extraFunction, extraWinId){
    //Gcoop.ProgressUrl = ajaxUrl;
    Gcoop.ProgressName = progressName;
    Gcoop.DisplayProgressCloseButton = showCloseButton;
    try{
        Gcoop.ExtraFunction = extraFunction == null || extraFunction == undefined ? null : extraFunction;
    }catch(err){
        Gcoop.ExtraFunction = null;
    }
    var currDate = new Date();
    var isWarnning = warnning ? "&warn=true" : "";
    var w_sheet_id = extraWinId == null || extraWinId == undefined ? "&w_sheet_id=" + Gcoop.GetCurrentPage() : "&w_sheet_id=" + extraWinId;
    var appProcess = "&application=" + Gcoop.GetApplication();
    var newUrl = Gcoop.GetUrl() + "Applications/keeping/dlg/w_dlg_running_progressbar.aspx?t=" + currDate.getMinutes() + currDate.getSeconds() + isWarnning + appProcess + w_sheet_id;
    Gcoop.OpenIFrame(450, 250, "w_dlg_running_progressbar.aspx", "", newUrl);
}

Gcoop.ParseFloat = function(text){
    return parseFloat(text);
}

Gcoop.ParseInt = function(text){
    if(text == "08"){
        return 8;
    } else if (text == "8"){
        return 8;
    } else if (text == "09"){
        return 9;
    } else if (text == "9"){
        return 9;
    } else {
        return parseInt(text);
    }
}

Gcoop.RemoveIFrame = function(){
    try{
        var ni = document.getElementById('iFrameMaster');
        var elNew = document.getElementById('iFrameChild');
        ni.removeChild(elNew);
    }catch(err){}
}

Gcoop.SetLastFocus = function(elementName){
    Gcoop.GetEl("tempLastFocus").value = elementName;
}

Gcoop.StringFormat = function(str, formats){
    if(!Gcoop.IsNum(str)){
        alert("ไม่สามารถแปลงค่าจากตัวอักษรเป็นตัวเลขได้");
        return str;
    }
    str = str + "";
    var formatted;
    var strformat = formats;
    var strformatlength = strformat.length;
    var strlength = str.length;
    if (strlength <= strformatlength) {
        formatted = strformat.substring(0, strformatlength - strlength) + str;
    }
    return formatted;
}

Gcoop.ToEngDate = function(tDate){
    try{
        return (Gcoop.ParseInt(tDate.substring(4, 8)) - 543) + "-" + tDate.substring(2, 4) + "-" + tDate.substring(0, 2);
    }catch(err){
        alert("รูปแบบวันที่ผิด");
        return "";
    }
}

Gcoop.Trim = function(text){
    return text.replace(/^\s+|\s+$/g, '');
}

function RemoveIFrame(){
    try{
        Gcoop.RemoveIFrame();
    }catch(err){}
}