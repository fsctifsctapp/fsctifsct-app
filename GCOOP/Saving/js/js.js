var oldCheckJs = true;
var isForceLoadSubmit = true;

var state;
var setobj = "div_list";
var slide = {
    tmo: null,
    opa: 100,
    duration: 140,
    frame: 8,
    process: false,
    active: function(a) {
        if (this.process) return;
        if (a == 'show') this.show();
        else this.hide();
    },
    show: function() {
        if (this.opa < 100) {
            this.process = true;
            this.opa += 100 / this.frame;
            this.setOpacity(this.opa, setobj);
            var _this = this;
            this.tmo = setTimeout(function() { _this.show() }, this.duration);
        } else {
            this.setOpacity(100, setobj);
            clearTimeout(this.tmo);
            this.process = false;

        }
    },
    hide: function() {
        if (this.opa > 0) {
            this.process = true;
            this.opa -= 100 / this.frame;
            this.setOpacity(this.opa, setobj);
            var _this = this;
            this.tmo = setTimeout(function() { _this.hide() }, this.duration);
        } else {
            clearTimeout(this.tmo);
            this.setOpacity(0, setobj);
            this.process = false;
        }
    },
    setOpacity: function(setOpacity, objectId) {
        var object = document.getElementById(objectId).style;
        object.opacity = (setOpacity / 100);
        object.MozOpacity = (setOpacity / 100);
        object.KhtmlOpacity = (setOpacity / 100);
        object.filter = "alpha(opacity=" + setOpacity + ")";
    }
}

function FullScreen() {
    moveTo(0, 0);
    resizeTo(screen.availWidth, screen.availHeight);
}

function openPopup(popupmenu) {
    target = document.getElementById(popupmenu);
    if (target.style.visibility == "hidden") {
        target.style.visibility = "visible";
    }
    else {
        target.style.visibility = "hidden";
    }
}

function closePopup(popupmenu) {
    var target = document.getElementById(popupmenu);
    target.style.visibility = "hidden";
}

/*============================ เริ่ม ส่วนของ Search =================================*/

function clearTXT(txt_search) {
    var target = document.getElementById(txt_search);
    target.select();
}
function StillShowDropdown() {
    document.getElementById('div_list').style.visibility = 'visible';
}

function IdelHideDropdown() {
    setTimeout("slide.active('hide');document.getElementById('div_list').style.visibility = 'hidden';", 5000);
    document.getElementById('txt_search').value = "type to search...";
}
function HideDropdown() {
    document.getElementById("div_list").style.visibility = "hidden";
    document.getElementById('txt_search').value = "type to search...";
    document.getElementById('txt_search').select();
}

function searchPage(txt_search, e) {
    var divlist = document.getElementById('div_list');
    var txtvalue = document.getElementById(txt_search).value; //ค่าที่ได้จาก Textbox
    var winobject = document.getElementById('ctl00_hidden_WIN_OBJECT').value; //รับค่าจาก Hidden Field hidden_WIN_OBJECT
    var windesc = document.getElementById('ctl00_hidden_WIN_DESCRIPTION').value; //รับค่าจาก Hidden Field hidden_WIN_DESCRIPTION
    var temp_str = "";
    var str_arr_winobject;
    var str_arr_windesc;
    var i = 0;
    var gotoUrl = GCOOP.GetUrl() + "Applications/" + state.Application + "/";
    var outputstr = "";

    str_arr_winobject = winobject.split(",");
    str_arr_windesc = windesc.split(",");

    if (e.keyCode != 13) {
        if (txtvalue != "") {
            //if (e.keyCode != 8) { //key 8 = backspace
            for (i; i < str_arr_windesc.length; i++) {
                var temp_arr = str_arr_windesc[i];
                if (temp_arr.search(txtvalue) >= 0) {
                    outputstr = outputstr + "<span id=\"link" + i + "\" style=\"font-size:small; cursor:pointer;\" onclick=\"window.location='" + gotoUrl + str_arr_winobject[i] + ".aspx'\">&nbsp;&nbsp;- " + str_arr_windesc[i] + "</span><br>";
                }
            }
            divlist.innerHTML = outputstr + "<center><span id=\"div_close\" style=\" background-color:ButtonFace; cursor:pointer;  text-align:center;\" onclick=\"HideDropdown()\">close</span></center>";
            divlist.style.visibility = "visible";
            //}
        }
    }

    if (txtvalue == "") {//ถ้า textbox เป็นค่าว่าง ให้ซ่อน Dropdows หรือ จะให้แสดงทุกหน้าจอ
        HideDropdown(); //ซ่อน Dropdown
        //        for (i; i < str_arr_windesc.length; i++) { //แสดงทุกหน้าจอ
        //            outputstr = outputstr + "<span id=\"link" + i + "\" style=\"font-size:small; cursor:pointer;\" onclick=\"window.location='" + gotoUrl + str_arr_winobject[i] + ".aspx'\">&nbsp;&nbsp;" + str_arr_windesc[i] + "</span><br>";
        //        }
        //        divlist.style.visibility = "visible";
        //        divlist.innerHTML = outputstr + "<center><span id=\"div_close\" style=\" background-color:ButtonFace; cursor:pointer; text-align:center;\" onclick=\"HideDropdown()\">close</span></center>";
    }


}
/*============================ จบ ส่วนของ Search =================================*/
function AddIFrameOnPost(){
    try{
        Gcoop.RemoveIFrame();
        var ni = document.getElementById('iFrameMaster');
        var newdiv = document.createElement('div');
        var divIdName = 'iFrameChild';
        newdiv.setAttribute('id', divIdName);
        ni.appendChild(newdiv);
        var elNew = document.getElementById(divIdName);
        
        elNew.style.top = 0; 
        elNew.style.left = 0;
        elNew.style.width = (screen.width - 20) + "px";
        elNew.style.height = "2500px";
        elNew.style.zIndex  = 999;
        elNew.style.position = "absolute";
        elNew.style.backgroundImage = "url('"+ Gcoop.GetUrl() +"img/tranparent.png')";
        elNew.innerHTML = "<input style='border:none;width:1px;height:1px;' type='text' id='ttttOnSubmit'><br /><br /><br /><br /><br /><br /><br /><br /><div align='center'><font size='12' color='orange'>โปรดรอสักครู่......</font></div>";
        window.document.getElementById("tempElementEnter").focus();
    }catch(error){}
}

function Page_LoadComplete() {
    state = new WebState();
    var webPageType = "";
    try{
        webPageType = document.getElementById("webPageType").value;
    } catch(Err){}
	//-- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- --
    try{
        SetThDateJavaScript01();
    } catch(Err){}
    
    if(webPageType != "w_dlg_xx_xxxxxx" && isForceLoadSubmit){
        this.__doPostBack = function(eventTarget, eventArgument) {
            AddIFrameOnPost();
            if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                theForm.__EVENTTARGET.value = eventTarget;
                theForm.__EVENTARGUMENT.value = eventArgument;
                theForm.submit();
            }
        }
    }
    
    // กลับไป Focus ที่เดิมเมื่อกด Enter แล้ว postBack
    // Firefox, Chrome, Safari
    try{
        var elLastFocusName = Gcoop.GetEl("tempLastFocus").value;
        var elFocus = document.aspnetForm.elements[elLastFocusName];
        elFocus.focus();
    }catch(err){}
    
    // เรียกใช้ SheetLoadComplete() ของไฟล์ w_sheet_xxxxxxxx.aspx
    try{
        if(webPageType == "w_dlg_xx_xxxxxx"){
            DialogLoadComplete();
        } else{
            SheetLoadComplete();
        }
    }catch(err){ }
    document.addEventListener("keyup", HandleOnKeyUp, true);
    //FullScreen();
}

function Open_Toolbar() {
    try {
        MenubarOpen();
    } catch (err) { alert("ไม่สามารถใช้ปุ่ม Open ได้"); return false; }

}

function Save_Toolbar() {
    try {
        MenubarSave();
    } catch (err) { alert("ไม่สามารถใช้ปุ่ม Save ได้"); return false; }
}

function New_Toolbar() {
    try {
        MenubarNew();
    } catch (err) { alert("ไม่สามารถใช้ปุ่ม New ได้"); return false; }
}


// ------------------ DOYS
function DwDateF(dwobj, thName, sender, row, name, newVar){
    if(oldCheckJs){
        alert("DwDateF ยกเลิกแล้ว");
        return;
    }
    if(thName == name){
        try{
            var nvTrim = Trim(newVar);
            if(nvTrim.length == 8){
                var dd = nvTrim.substring(0, 2);
                var mm = nvTrim.substring(2, 4);
                var yyyy = nvTrim.substring(4);
                if(!IsNum(dd)) throw "Edd";
                if(!IsNum(mm)) throw "Emm";
                if(!IsNum(yyyy)) throw "Eyyyy";
                var result = dd + "/" + mm + "/" + yyyy;
                dwobj.SetItem(row, thName, result);
                dwobj.AcceptText();
            }
        }catch(Err){
            alert(Err);
            dwobj.SetItem(row, name, dwobj.GetItem(row, name));
        }
    }
}

function DwThDate(enName, thName, dwobj, sender, row, name, newVar){
    if(oldCheckJs){
        alert("DwThDate ยกเลิกแล้ว");
        return;
    }
    if(thName == name){
        try{
            var nvTrim = Trim(newVar);
            if(nvTrim.length == 8){
                var dd = nvTrim.substring(0, 2);
                var mm = nvTrim.substring(2, 4);
                var yyyy = nvTrim.substring(4);
                if(!IsNum(dd)) throw "Edd";
                if(!IsNum(mm)) throw "Emm";
                if(!IsNum(yyyy)) throw "Eyyyy";
                var result = dd + "/" + mm + "/" + yyyy;
                var newDate = (ParseInt2(yyyy)-543) + "-" + mm + "-" + dd;
                dwobj.SetItem(row, enName, newDate);
                dwobj.AcceptText();
            }
        }catch(Err){
            alert(Err);
            dwobj.SetItem(row, name, dwobj.GetItem(row, name));
        }
    }
}

function IsNum(number){
    if(oldCheckJs){
        alert("IsNum ยกเลิกแล้ว Gcoop.IsNum");
        return;
    }
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
            if(digit >="0" && digit <="9"){
                //
            }else{
                return false;
            }
        }
        return true;
    }catch(Err){return false;}
}

function ParseInt2(text){
    if(oldCheckJs){
        alert("ParseInt2 ยกเลิกแล้ว Gcoop.ParseInt");
        return;
    }
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

function IsValid(text) {
    if(oldCheckJs){
        alert("IsValid ยกเลิกแล้ว Gcoop.IsValid");
        return;
    }
    try{
        if(text != "undefined" && text != null && text != ""){
            return true;
        } else {
            return false;
        }
    }catch(err){ return false;}
}

function Trim(text){
    if(oldCheckJs){
        alert("Trim ยกเลิกแล้ว Gcoop.Trim");
        return;
    }
    return text.replace(/^\s+|\s+$/g, '');
}

function Explode(bitVar, fullVar){
    if(oldCheckJs){
        alert("Explode ยกเลิกแล้ว Gcoop.Explode");
        return;
    }
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

function StringFormat(str, formats) {
    if(oldCheckJs){
        alert("StringFormat ยกเลิกแล้ว Gcoop.StringFormat");
        return;
    }
    if(!IsNum(str)){
        alert("ไม่สามารถแปลงค่าจากตัวอักษรเป็นตัวเลขได้");
        return str;
    }
    var formatted;
    var strformat = formats;
    var strformatlength = strformat.length;
    var strlength = str.length;
    if (strlength <= strformatlength) {
        formatted = strformat.substring(0, strformatlength - strlength) + str;
    }
    return formatted;
}

//ใช้สำหรับ เปิด Dialog
//รับ argument ดังนี้ (ความกว้างของ dlg , ความสูงของ dlg , หน้าที่ต้องการเปิด.aspx , argument ที่ต้องการส่ง ex. "?memno=123&type=0" ถ้าไม่ต้องการส่งให้ใส่ค่าว่าง "")
function opendlg(dlgwidth, dlgheight, pageaspx, sendargument){
    if(oldCheckJs){
        alert("opendlg ยกเลิกแล้ว Gcoop.OpenDlg");
        return;
    }
    var left = (screen.width/2)-(dlgwidth/2);
    var top = (screen.height/2)-(dlgheight/2);
    top = (top-40) < 0 ? top : top - 40;
    var getUrl = document.getElementById("ctl00_HUrl").value;
    var getApp = document.getElementById("ctl00_HApplication").value;
    if(getUrl == null){
        alert("PageLoad not ready complete");
    }else{
        var dlgurl = getUrl + "Applications/" + getApp + "/dlg/" + pageaspx + sendargument;
        window.open(dlgurl, null, "height="+dlgheight+",width="+dlgwidth+",top="+top+",left="+left+",status=0,toolbar=0,menubar=0,location=0,resizable=0");
    }
}

function loadUniquePage(page) {
    if (opener && !opener.closed){
        opener.focus();
    }
    else {
        var myWin = window.open(page);
        opener = myWin;
    }
}

function runReport() { 
    var ws = new ActiveXObject("WScript.Shell"); 
    ws.run("file:///C:/GCO/SAVING/sETP/BKCAT/Zconfig/reportdeposit.exe"); 
}

function GetDwThColumnDateName(tDwName){
    var ext = new Array();
    ext = tDwName.split('_');
    return tDwName.substring(0, tDwName.length - (ext[ext.length - 1].length + 1));
}

function GetDwThColumnDateRow(tDwName){
    var ext = new Array(); //tDwName.split('_');
    ext = tDwName.split('_');
    return parseInt( ext[ext.length - 1] ) + 1;
}

function HandleOnKeyUp(e) {
    try{
        OnKeyUpBegin(e, e.target);
    }catch(err){}
    try{
        var isAcceptText = false;
        if (e.keyCode == '13') {
            var cName = "";
            var cEnName = "";
            var cRow = "";
            var element = e.target;
            var valueIs = element.value;
            for(i = 0; i < dwColumnNameArrayCount; i++){
                if(element.name == thDwColumnNameArray[i]){
                    var ii = 0;
                    cName = GetDwThColumnDateName(thDwColumnNameArray[i]);
                    cEnName = GetDwThColumnDateName(enDwColumnNameArray[i]);
                    cRow = GetDwThColumnDateRow(thDwColumnNameArray[i]);
                    
                    valueIs = Gcoop.Trim(valueIs);
                    
                    while(valueIs.indexOf("/") >= 0){
                        valueIs = valueIs.replace("/", "");
                        ii++;
                        if(ii > 10000) break;
                    }
                    
                    //dwObjectJavaScriptArray[i].SetItem(cRow, cName, valueIs);
                    //dwObjectJavaScriptArray[i].AcceptText();

                    var dd = valueIs.substring(0, 2);
                    var mm = valueIs.substring(2, 4);
                    var yyyy = valueIs.substring(4);
                    
                    try{
                        if(!Gcoop.IsNum(dd)) throw "dd";
                        if(!Gcoop.IsNum(mm)) throw "mm";
                        if(!Gcoop.IsNum(yyyy)) throw "yyyy";
                    }catch(err){ 
                        alert(err); 
                        return; 
                    }
                    
                    valueIs = (parseInt(yyyy + "") - 543)  + "-" + mm + "-" + dd;
                    
                    element.value = dd + "/" + mm + "/" + yyyy
                    isAcceptText = true;
                    break;
                }
            }
            
            Gcoop.GetEl("tempLastFocus").value = element.name;
            Gcoop.GetEl("tempElementEnter").focus();
            
            if(isAcceptText){
                dwObjectJavaScriptArray[i].SetItem(cRow, cEnName, valueIs);
                dwObjectJavaScriptArray[i].AcceptText();
            }
            
            element.focus();
            element.select();
        }
    }catch(err){}
    try{
        OnKeyUpEnd(e, e.target);
    }catch(err){}
    try{
        if(e.keyCode == "123"){
            alert(Gcoop.GetLastFocus());
        } else if (e.keyCode == "120"){
            MenubarSave();
        } else if (e.keyCode == "113"){
            MenubarNew();
        } else if (e.keyCode == "119"){
            MenubarOpen();
        }
    }catch(err){
    }
    //ทดสอบ
}