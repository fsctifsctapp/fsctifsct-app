var Ajax = {
    getAjaxOb: function() {
        var XMLHttpRequestObject = false;
        if (window.XMLHttpRequest) {
            XMLHttpRequestObject = new XMLHttpRequest();
        } else if (window.XMLHttpRequest) {
            XMLHttpRequestObject = new ActiveXObject("Microsoft.XMLHTTP");
        }
        return XMLHttpRequestObject;
    },

    toGet: function(url, callFunction, leftt) {
//        if (data.indexOf("'") > 0) {
//            alert("Can not use ' ");
//            return;
//        }
//        if (leftt == true) {
//            //document.getElementById("processing").innerHTML = "<img src=\"img/blog24.jpg\" width=\"24\" height=\"24\" />";
//        } else {
//            document.getElementById("processing").innerHTML = "<img src=\"img/D360.gif\" width=\"16\" height=\"16\" />";
//        }
        var ajaxOb = this.getAjaxOb();
        var days = new Date();
        if (ajaxOb) {
            ajaxOb.open("GET", url + "&dddd=" + days.getMinutes() + days.getSeconds());
            ajaxOb.onreadystatechange = function() {
                if (ajaxOb.readyState == 4 && ajaxOb.status == 200) {
                    callFunction(ajaxOb.responseText.replace(/^\s+|\s+$/g, ''));
                    delete ajaxOb;
                    ajaxOb = null;
                }
            }
            ajaxOb.send(null);
        }
    },
    toPost: function(url, data, callback) {
//        if (data.indexOf("'") > 0) {
//            alert("Can not use ' ");
//            return;
//        }
//        document.getElementById("processing").innerHTML = "<img src=\"img/D360.gif\" width=\"16\" height=\"16\" />";
        var ajaxOb = this.getAjaxOb();
        if (ajaxOb) {
            ajaxOb.open("POST", url);
            ajaxOb.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            ajaxOb.onreadystatechange = function() {
                if (ajaxOb.readyState == 4 && ajaxOb.status == 200) {
                    callback(ajaxOb.responseText.replace(/^\s+|\s+$/g, ''));
                    delete ajaxOb;
                    ajaxOb = null;
                }
            }
        }
        ajaxOb.send(data);
    }
}