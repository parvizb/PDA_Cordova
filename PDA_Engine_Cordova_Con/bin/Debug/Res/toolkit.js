function DoBatch(data, Batch,FnOk,FnFail) {
    db.transaction(function (tx) {
        var ScallerValues = new Array();
        for (var l = 0; l < Batch.Commands.length; l++) {
            ScallerValues.push("");
        }
        for (var l = 0; l < Batch.Commands.length; l++) {
            console.log("l:" + l);
            var Com = Batch.Commands[l];
            var initValues = new Array();
            for (var j = 0; j < Com.Parameters; j++) {
                if (Com.Parameters[j].sourceType == 'ScallerValues') {
                    initValues.push(toInput(Com.Parameters[j].name, intParse(ScallerValues[Com.Parameters[j].sourceTypeParameter])));

                }

            }

            for (var m = 0; m < Com.Parameters.length; m++) {
                console.log("m:" + m +" Com " + Com.DBCommand);
                var index = -1;
                for (var n = 0; n < Math.min(2, data[l].length) ; n++) {
                    console.log("n:" + n);
                    for (var n2 = 0; n2 < data[l][n].length; n2++) {
                        console.log("n2:" + n2);
                        for (var n3 = 0; n3 < data[l][n][n2].length; n3++) {
                            console.log("n3:" + n3);
                            if (data[l][n][n2][n3].key == Com.Parameters[m].name) {
                                index = n3;

                            }
                        }
                    }

                }
                var re = Com.Parameters[m].RealData;
                var Def = Com.Parameters[m].DefaultValueSource;
                if (index != -1) {
                    
                    for (var ri = 0; ri < data[l].length; ri++) {
                        console.log("ri:" + ri);
                        for (var ri2 = 1; ri2 < data[l][ri].length; ri2++) {
                            console.log("ri2:" + ri2);
                                var noValue = false;
                                if (data[l][ri][ri2][index].value == "") {
                                    noValue = true;

                                }
                                if (isNaN(data[l][ri][ri2][index].value)) {
                                    noValue = true;

                                }
                                if ((re == "Date") || (re == "InputDate")) {
                                    data[l][ri][ri2][index].value = NorDate(data[l][ri][ri2][index].value);
                                }
                                if ((re == "Money") || (re == "InputMoney") || (re == "Integer") || (re == "InputInteger")) {
                                   
                                    data[l][ri][ri2][index] = toInput(data[l][ri][ri2][index].key, data[l][ri][ri2][index].value, false);

                                }
                                if ((re == "CheckBox")) {
                                    data[l][ri][ri2][index].value == ConvertBit(data[l][ri][ri2][index].value);

                                }
                                if (noValue == true) {
                                    if (Def == "Static") {
                                        data[l][ri][ri2][index].value = Com.Parameters[m].DefaultValueParameter;

                                    }
                                    if (Def == "NULL") {
                                        data[l][ri][ri2][index].value = null;

                                    }
                                    if (Def == "Expr") {
                                        data[l][ri][ri2][index].value = eval(Com.Parameters[m].DefaultValueParameter);

                                    }
                                }

                            }

                        }
                        
                    
                }
                
                      
            }
          
        
         
            for (var c = 0; c < data[l].length; c++) {
                console.log("c:" + c);
            
                for (var k = 1; k < data[l][c].length; k++) {
                    var tcom = Com.DBCommand.substring(1);
                    for (var k2 = 0; k2 < data[l][c][k].length; k2++) {
                       
                        tcom = AllReplace(tcom, "@" + data[l][c][k][k2].key, "'" + (data[l][c][k][k2].value === undefined ? '' : data[l][c][k][k2].value) + "'");

                    }
                    for (var zz = 0; zz < initValues.length; zz++) {
                        console.log("zz:" + zz);
                        tcom = AllReplace(tcom, "@" + initValues[zz].key, "'" + (initValues[zz].value === undefined ? '' : initValues[zz].value) + "'");


                    }
                    tx.executeSql(tcom, [], function (tx, results) {
                        if (results.rows.length > 0) {
                            ScallerValues[l] = results.rows[0][0];
                        }

                    }, function () { });
                }
              
          

            }


        }
       
        var d = new Object();
        d.code = 0;
        d.returnValue = 0;
        d.Message = "با موفقیت ثبت شد";
        FnOk(d);
        $('#loadingBar').hide();

    });

}

var dbg = null;

function Ajax(pageName, Mehtod, jData, fnOk, fnFail) {

    $('#loadingBar').show();
    var obj = JSON.parse(jData);
    var Command = "";
    if (Mehtod == "ScallerSubmit") {
        Command = DBAction(obj.PageName, Mehtod);
        Scaller(Command, obj.Parameters, fnOk, fnFail);
    }
    if (Mehtod == "getTableViewRecords") {
        Command = DBAction(obj.PageName, Mehtod);
        ReadTable(Command, obj.Parameters, fnOk, fnFail);

    }
    if (Mehtod == "getStartValueFromServer") {
        Command = DBAction(obj.PageName, Mehtod);
        ReadTable(Command, obj.Parameters, fnOk, fnFail);

    }
    if (Mehtod == "AjaxAction") {
        Command = DBAction("aa_" + obj.actionName, "AjaxAction");
        Scaller(Command, obj.Parameters, fnOk, fnFail);
    }
    if (Mehtod == "getDBSelect2DirectValue") {
        var data = obj;
        var Com = DBAction(data.PageName + '@' + data.PageParameterName + "_direct", Mehtod);
        var setup = Select2Prop(data.PageName, data.PageParameterName,false);
        var o = [{ key: setup.DirectParameterName, value: data.value }];
        ReadTable(Com, o, function (r) {
            var x = JSON.parse(r.retrunValue);
            dbg = x.Records[0];
            var z3 = { value:x.Records[0][ Object.keys( x.Records[0])[0]]  }
          
            

            fnOk(z3);
        }, fnFail);

    }
    if ((Mehtod == 'Select2Ajax') || (Mehtod=='Select2AjaxTable')) {
        var data = obj;
        var setup = Select2Prop(data.PageName, (Mehtod == "Select2Ajax" ? data.ParaName : data.colname), (Mehtod != "Select2Ajax"));
        ReadTable(setup.Command, data.Parameters, function (da) {
            var results = new Array();
            for (var q = 0; q < setup.options; q++) {
                results.push(setup.options[q]);
            }
            var relData = JSON.parse(da.retrunValue);
            for (var q = 0; q < relData.Records.length; q++) {
                var re = new Object();
                re.id = relData.Records[q][setup.idcol];
                re.text = relData.Records[q][setup.textcol];
                results.push(re);
            }

             
        fnOk(results)}, function () { });
    }
    if ((Mehtod == 'BatchCommand')) {
        var data = obj;
        DoBatch(data.records, GetBatchInfo(data.PageName, data.CommandName),fnOk,fnFail);

    }
}
var trp = null;
$.ajax = function (options) {
    
    var o = options;
    var data=JSON.parse(o.data);
    if (o.url == "/Home/Select2Ajax") {
       var setup=  Select2Prop(data.PageName, data.ParaName, false);
       ReadTable(setup.Command, data.Parameters, function (da) {
           var results = new Array();
           for (var q = 0; q < setup.options; q++) {
               results.push(setup.options[q]);
           }
           var relData = JSON.parse(da.retrunValue);
           for (var q = 0; q < relData.Records.length; q++) {
               var re = new Object();
               re.id = relData[q][setup.idcol];
               re.text = relData[q][setup.textcol];
               results.push(re);
           }

          trp= options.transport;
       }, function () { });
    }
}

function ConvertBit(val) {
    return val == true ? "1" : "0";

}
function NorDate(date) {

    var parts = date.split('/');
    return PadLeft(parts[0], 4) + "/" + PadLeft(parts[1], 2) + "/" + PadLeft(parts[2], 2);
}
 


function SetDefaultValue(paras,key, defaultVale) {
    var index = -1;
    for (var i = 0; i < paras.length; i++) {
        if (paras[i].key == key) {
            index = i;
            break;
        }

    }
    if (index == -1) {
        paras.push(toInput(key, defaultVale))

    }
    else
    {
        if (!(paras[index].value.toString() !="")) {
            paras[index].value = defaultVale;

        }
        if ( isNaN(  paras[index].value )==true) {
            paras[index].value = defaultVale;

        }
    }

}


function PadLeft(v, len) {
    var t = "";
    var difflen = len - v.length;
    for (var i = 0; i < difflen; i++) {
        t += "0";

    }
    return t + v;
}


function Scaller(command,parameters,fnOk,fnFail) {

    var com = command;
    for (var i = 0; i < parameters.length; i++) {
        com = AllReplace(com, "@" + parameters[i].key, "'" + parameters[i].value + "'");
    }
    console.log(com);
    db.transaction(function (tx) {
        tx.executeSql(com, [], function (tx, results) {
            var da = new Object(); da.Message = 'با موفقیت انجام شد'; da.code = 0;
            $('#loadingBar').hide();
            da.retrunValue = null;
            if (results.rows.length > 0) {
                da.retrunValue = results.rows[0][0];

            }
            fnOk(da)
        }, function (a, err) { $('#loadingBar').hide(); console.log(err.message); fnFail(a, err.message); }

            );

    });
}
function ReadTable(command, parameters, fnOk, fnFail) {

    var com = command;
    for (var i = 0; i < parameters.length; i++) {
        
        com = AllReplace(com, "@" + parameters[i].key, "'" + (parameters[i].value===undefined?"":parameters[i].value) + "'");
    }
    db.transaction(function (tx) {
        tx.executeSql(com, [], function (tx, results) {
            var da = new Object(); da.Message = 'با موفقیت انجام شد'; da.code = 0;
            $('#loadingBar').hide();
            var ox=new Object();
            ox.Records = new Array();
            for (var i = 0; i < results.rows.length; i++) {

                ox.Records.push(results.rows[i]);

            }
            da.retrunValue=JSON.stringify(ox);
            fnOk(da)
        }, function (a, err) { console.log(err.message); $('#loadingBar').hide(); fnFail(a, err.message); }

            );

    });
}
function DBAction(pageName,method) {
        


}



var pagingArray = new Array();
function AllReplace(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}


function GenPagingLinks() {
    pagingArray = new Array();
    var q = $('[rowcount]');
    if (q.length == 0) {
        return;
    }
    else {
        var counter = parseInt(q.val());
        var i = 1;
        var j = 1;
        while (i <= totalRecords) {
            pagingArray.push({ start: i, end: i + counter ,caption:j , isCurrent:  i==fromRecords  ,showit :  Math.abs (  i - (fromRecords)  ) <(10*counter)    });
            i += counter;
            j += 1;
        }

        if (toRecords > totalRecords) {
            toRecords = totalRecords;
        }
        if (pagingArray.length > 0) {
            pagingArray[0].showit = true;
            pagingArray[pagingArray.length - 1].showit = true;
        }
        currentScope.pagingArray = pagingArray;
        currentScope.PaginCounter = counter;
        PagingMover();
    }

}
function PagingMover() {

    currentScope.fromRecords = fromRecords;
    currentScope.toRecords = toRecords;
    currentScope.totalRecords = totalRecords;
 
    currentScope.$apply({});
}
function goPaging(start) {
    
    fromRecords =parseInt( start);
    var q = $('[rowcount]');
    if (q.length == 0) {
        toRecords = 9999999999999999;
    }
    else {
        toRecords =  fromRecords + parseInt(q.val()) - 1;

    }
 
    window.CurrentSerachMethod();
    PagingMover();
}



function resetPaging() {
    fromRecords = 1;
    pagingArray = new Array();
    currentScope.pagingArray = pagingArray;
    var q= $('[rowcount]');
    if(q.length==0)
    {
        toRecords=9999999999999999;
    }
    else
    {
        toRecords =fromRecords + parseInt( q.val()  )-1;
    }
    totalRecords = 0;
}
var fromRecords = 1;
var toRecords = 999999999999;
var totalRecords = 0;

var Validator = new Object();
var Messager = new Object();
Messager.errors = new Array();
Messager.info = new Array();
Validator.ClearErrors = function () {
    Messager.errors = new Array();

}
Messager.ClearInfos = function () {
    Messager.info = new Array();



}
mergeArray = function (a1, b1) {
    console.log(a1);
    console.log(b1);
    var keys = Object.keys(b1);
    for (var i = 0; i < keys.length; i++) {
        a1[keys[i]] = b1[keys[i]];

    }
   
}


Validator.CheckEmpty = function (id, caption, row) {
    var val = $("#" + id).val();
    if (val == '') {
        $('#' + id).css('background', 'pink');
        Messager.errors.push((row == null ? "" : " ردیف: " + row + " ") + 'کادر ' + caption + ' نمی تواند خالی رها شود ');
    }
    else {
        $('#' + id).css('background', '');
    }
}


Validator.CheckStringLength = function (id, caption, len, row) {
    var val = $("#" + id).val();
    if (val.length != len) {
        $('#' + id).css('background', 'pink');
        Messager.errors.push((row == null ? "" : " ردیف: " + row + " ") + ' طول عبارت  ' + caption + ' بایستی ' + len + ' حرف یا رقم باَشد');
    }
    else {
        $('#' + id).css('background', '');
    }
}
Validator.CheckRegDate = function (id, caption, len, row) {
    var val = $("#" + id).val();
    if (val == '') {
        $('#' + id).css('background', 'pink');
        Messager.errors.push((row == null ? "" : " ردیف: " + row + " ") + 'کادر ' + caption + ' نمی تواند خالی رها شود ');
    }
    else {
        $('#' + id).css('background', 'pink');
        var parts = val.split('/');
        var year = parseInt(parts[0]);
        var month = parseInt(parts[1]);
        var day = parseInt(parts[2]);
        if ((year < 1300) || (year > 9999) || (month < 1) || (month > 12) || (day < 1) || (day > (month < 7 ? 31 : 30))) {
            Messager.errors.push('تاریخ وارد شده در  ' + caption + ' صحیح نیست ');
            return;
        }


        $('#' + id).css('background', '');
    }


}
Validator.CheckRegInteger = function (id, caption, len, row) {
    var val = $("#" + id).val().replace(/,/g,'');
    if (val == '') {
        $('#' + id).css('background', 'pink');
        Messager.errors.push((row == null ? "" : " ردیف: " + row + " ") + 'کادر ' + caption + ' نمی تواند خالی رها شود ');
    }
    else {
        $('#' + id).css('background', 'pink');

        if ((val == "") || (isNaN(val))) {
            Messager.errors.push('عدد وارد شده در  ' + caption + ' صحیح نیست ');
            return;
        }


        $('#' + id).css('background', '');
    }


}
Validator.CheckRegFloat = function (id, caption, row) {
    var val = $("#" + id).val();
    if (val == '') {
        $('#' + id).css('background', 'pink');
        Messager.errors.push((row == null ? "" : " ردیف: " + row + " ") + 'کادر ' + caption + ' نمی تواند خالی رها شود ');
    }
    else {
        $('#' + id).css('background', 'pink');

        if ((val == "") || (isNaN(parseFloat(val)))) {
            Messager.errors.push('عدد وارد شده در  ' + caption + ' صحیح نیست ');
            return;
        }


        $('#' + id).css('background', '');
    }


}
Validator.CheckRegSelect2 = function (id, caption,row) {
    var val = $("#" + id).val();
    if (val == null) {
        $('#' + id).css('background', 'pink');
        Messager.errors.push((row == null ? "" : " ردیف: " + row + " ") + 'کادر ' + caption + ' نمی تواند خالی رها شود ');
    }
    else {
        $('#' + id).css('background', '');
    }


}
function BackPage() {
    if (getDailOpen() == "") {


        window.history.back(0);
        setTimeout(LoadCache, 1000);
    }
    else {
        return $("#mdl" + getDailOpen()).modal('hide');

    }
}

Validator.ShowErrors = function () {
    $('#myModalLabel').html('خطا');
    var str = '';
    for (var l = 0; l < Messager.errors.length; l++) {
        str += Messager.errors[l] + "</br>";

    }
    $('.modal-body[n]').html(str);
    $('#myModal').modal()
}
Validator.RegEmail = function (id, caption) {


    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var r = re.test($('#' + id).val());
    if (r == false) {
        $('#' + id).css('background', 'pink');
        Messager.errors.push(' پست الکترونیک وارد شده در  ' + caption + ' صحیح نیست ');
    }
    else {
        $('#' + id).css('background', '');
    }

}
Messager.ShowInfo = function (title) {
    $('#myModalLabel').html(title);
    var str = '';
    for (var l = 0; l < Messager.info.length; l++) {
        str += Messager.info[l] + "</br>";

    }
    $('.modal-body[n]').html(str);
    $('#myModal').modal()
}
function LoadCache() {
  
        if (Dic[document.title] != null) {
            CurrentSerachMethod(null, Dic[document.title]);
        }
        
 
}
function TableViewAjax(name, data, fnOk, fnFailOk) {
    Ajax('Home', name, JSON.stringify(data), function (dx) {

        if (dx.code != 0) {
            Validator.ClearErrors();
            Messager.errors.push(dx.Message);
            Validator.ShowErrors();
            fnFailOk(dx.Message);
            if (dx.code == 401) {
                setTimeout(function () { window.location='/home/LogIn' }, 2000);

            }
        }
        else {
            dx.records = JSON.parse(dx.retrunValue).Records;
            for (var l = 0; l < dx.records.length; l++) {
                dx.records[l].RowState = 'NoChange';
                dx.records[l].selected = false;
                dx.records[l].rndId = Math.round(Math.random() * 99999999999999);


            }
      
            fnOk(dx);
        
        }

    },
    function (data) {

        Messager.errors.push('خطا نامشخص');
        Validator.ShowErrors();
        fnFailOk('خطا نا مشخص');
    })



}

function ScallerAjax(name, data, fnOk, fnFailOk) {
    Ajax('Home', name, JSON.stringify(data), function (dx) {

        if (dx.code != 0) {
            Validator.ClearErrors();
            Messager.errors.push(dx.Message);
            Validator.ShowErrors();
            fnFailOk(dx.Message);
            if (dx.code == 401) {
                setTimeout(function () { window.location = '/home/LogIn' }, 2000);

            }
        }
        else {
            fnOk(dx);

        }

    },
    function (data) {

        Messager.errors.push('خطا نامشخص');
        Validator.ShowErrors();
        fnFailOk('خطا نا مشخص');
    })



}


function ScallerAjax(name, data, fnOk, fnFailOk) {
    Ajax('Home', name, JSON.stringify(data), function (dx) {

        if (dx.code != 0) {
            Validator.ClearErrors();
            Messager.errors.push(dx.Message);
            Validator.ShowErrors();
            fnFailOk(dx.Message);
            if (dx.code == 401) {
                setTimeout(function () { window.location = '/home/LogIn' }, 2000);

            }
        }
        else {
            fnOk(dx);

        }

    },
    function (data) {

        Messager.errors.push('خطا نامشخص');
        Validator.ShowErrors();
        fnFailOk('خطا نا مشخص');
    })



}


Messager.ShowMessage = function (title, message) {
    Messager.ClearInfos();
    Messager.info.push(message);
    Messager.ShowInfo(title);

}
function toInput(key, value,isstring) {
    var obj = new Object();
    obj.key = key;
    obj.value = value;
    obj.isstring = true;
    if (isstring == false) {
        obj.value = parseInt(AllReplace(obj.value, ",", ""));

    }
    return obj;
}

function ConfirmAsk(message, strFun) {
    $('#myModalLabel').html('تایید');
    $('.modal-body').html(message);
    $('#myModalYesNo').modal();
    window.confirmFun = strFun;
}

function NormalResult() {
    $('a[pdaajaxsyntax]').each(function () { $(this).attr('onClick', $(this).attr('pdaajaxsyntax')) });
    $('[moneytype="yes"]').each(function () { NumberInput(null, $(this)[0]) });
}

function In(value) {
    for (var l = 1; l < arguments; l++) {
        if (value == arguments[l]) {
            return true;
        }
    }
    return false;
}
var targetFuns = null;
var Plen = 0;




function confirmInputBox() {
    var arrays = new Array();
    for (var l = 0; l < Plen; l++) {
        arrays.push($('#arg' + l).val());

    }
    targetFuns(arrays);
     
}

function InputBox(title,captions, fun) {
    Plen = captions.length;
    targetFuns = fun;

    var s = '';
   
    for (var l = 0; l < captions.length; l++) {
        s += ' <form class="form-horizontal"  role="form"> <div class="form-group"> <label class="col-sm-3  control-label " > ' + captions[l] + ' </label>  <div  class="col-sm-9" ><input type="text"   class="form-control" id="arg' + l + '" /></div></div></form>';
    }
  
 
    $('#DInputBoxTitle').html(title);
    $('[name="inputBody"]')[0].innerHTML = s;
    $('#DInputBox').modal();
}

function Num(v) {

    try {
        if (v === undefined) {
            return 0;
        }
        v = v.toString();
        return parseFloat(v.replace(/,/g, ''));
    }
    catch (ex) {
        return 0;

    }

}

function blockNonIntNumber(e) {
    if (e.charCode == 0) {

        return true;
    }



    if (e.charCode == 45) {

        return true;
    }
    if (e.charCode < 48) {

        return false;
    }
    if (e.charCode > 57) {

        return false;
    }
    return true;
}
function blockNonTimeNumber(e) {
    if (e.charCode == 0) {

        return true;
    }
    if (e.charCode == 58) {

        return true;
    }
    if (e.charCode < 48) {

        return false;
    }
    if (e.charCode > 57) {

        return false;
    }
    return true;

}
function ReformatDate(con) {
    var r = $('#' + con).val();
    if (r.length == 8) {
        par = r.split('/');
        if (par.length == 1) {
            $('#' + con).val(r.substring(0, 4) + '/' + r.substring(4, 6) + '/' + r.substring(6, 8));

        }

    }
}

function blockNonDateNumber(e) {


    if (e.charCode < 47) {

        return false;
    }
    if (e.charCode > 57) {

        return false;
    }
    return true;

}
var sss;
var sss;
var dbg = null;
function Select2AjaxInit(id, fun,pagename,paraname) {

    var r = $('#' + id).select2({
        ajax: {
              
            transport: function (params, success, failure) {
                Ajax(null, 'Select2Ajax', params.data, function (da) { dbg = success; var di = new Object(); di.results = da; success(di) }, function () { });

            },
            quietMillis: 100,
            data: fun,
            processResults: function (data) {
                sss = data.results;


                return data;
            }

        }
    }
     );

    return r;

}
function Select2AjaxInitTable(id, fun) {
    var valx = $('#' + id).val();
    var r = $('#' + id).select2({

        ajax: {

            transport: function (params, success, failure) {
                Ajax(null, 'Select2AjaxTable', params.data, function (da) { dbg = success; var di = new Object(); di.results = da; success(di) }, function () { });

            },
            quietMillis: 100,
            data: fun,
            quietMillis: 100,
            data: fun,
            processResults: function (data) {
                sss = data.results;


                return data;
            }

        }
    });
    
    return r;

}
function NumberInput(e, t) {
    t.value = t.value.replace(/,/g, '');
    t.value = addCommas(t.value);

}
function ShowAsMoney(v) {
 
    v = v.toString().replace(/,/g, '');
    v = addCommas(v);
    return v;
}
function ShowBoolean(v) {
    switch (v) {
        case 'True':
            return 'بله';
            break;
        case 'False':
            return 'خیر';
            break;
        case '':
            return 'نا مشخص';
            break;

    }

}
//درج کاما برای عدد
function addCommas(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}


function TinyMceEditor(id) {

    tinymce.EditorManager.remove();
    tinymce.init({ selector: '#' + id, plugins: 'print preview fullpage searchreplace autolink directionality  visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime lists textcolor wordcount imagetools contextmenu colorpicker textpattern help', toolbar1: 'formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat', language: 'fa_IR' });


}
function goToLink(link) {
    var d = document.createElement('a');
    d.href = link;
    document.body.appendChild(d);
    d.click();

    document.removeChild(d);
}

function getDailOpen() {
    var r = "";
    $('div[isDailog]').each(function () {
        if ($(this)[0].style.display == 'block') {
            r= $(this).attr('PageName');
        }
      
       

    });

    return r;
}

var ScopeDlg = new Array();
function Para(id) {
 
    var d = getDailOpen();
    r = document.getElementById('txt' + (d!=""? d : window.pageName) + id);
    if (r != null) {
        if (r.getAttribute('Type') == 'Number') {
            return parseFloat($('#' + r.id).val());
        }
        else {
            if ($('#txt' + id).attr('moneymode') != 'yes') {
                return $('#' + r.id).val();
            }
            else {

                return Num(   $('#' + r.id).val());
            }
         
        }
    }
    else {
        return $('#' + r.id).val();

    }

}
function Select2AjaxMultValuesSetStatic(controllId, records, valueColumn) {
    var s = document.getElementById(controllId);

    var vals = new Array();
    for (var l = 0; l < records.length; l++) {
     
        vals.push(records[l][valueColumn]);
      
    }
    $('#' + controllId).val(vals).trigger('change');
}
function Select2AjaxMultValuesSet(controllId,records,valueColumn,titleColumn) {
    var s=document.getElementById(controllId);
    s.innerHTML = "";
    var vals = new Array();
    for (var l = 0; l < records.length; l++) {
        var o = document.createElement('option');
        o.value = records[l][valueColumn];
        o.innerHTML = records[l][titleColumn];
        vals.push(records[l][valueColumn]);
        s.appendChild(o);
    }
    $('#' + controllId).val(vals).trigger('change');
}

function Select2AjaxDirect(pageName,pageParameter,stringValue,controllId) {
    var o=new Object();
    o.PageName=pageName;
    o.PageParameterName=pageParameter;
    o.value=stringValue;

    Ajax("Home", "getDBSelect2DirectValue", JSON.stringify(o), function (data) {
        var o = document.createElement('option');
        o.value = stringValue;
        o.innerHTML = data.value;
     
      
        document.getElementById(controllId).innerHTML = "";
        document.getElementById(controllId).appendChild(o);

        $('#' + controllId).trigger('change');

    },
    function (d) {
        if (d.status == 200) {


        }
        else {
            alert('خطا دریافت اطلاعات');

        }
    });
}


function GetElement(id) {

    var d = getDailOpen();
    r = document.getElementById('txt' + (d != "" ? d : window.pageName) + id);
    
    return r;
}
function Query(id) {

    return routeParams[id];
}

var JsEventInterface = new Object();
JsEventInterface.beforeValidate = null;
JsEventInterface.afterVaildate = null;
JsEventInterface.BeforeSubmit = null;
JsEventInterface.AfterSubmit = null;
JsEventInterface.BeforeOkReqSubmit = null;
JsEventInterface.AfterOkReqSubmit = null;
JsEventInterface.BeforeFailReqSubmit = null;
JsEventInterface.AfterFailReqSubmit = null;
JsEventInterface.BeforeReadTable = null;
JsEventInterface.AfterReadTable = null;
JsEventInterface.BeforeOkReqReadTable = null;
JsEventInterface.AfterOkReqReadTable = null;
JsEventInterface.BeforeFailReqReadTable = null;
JsEventInterface.AfterFailReqReadTable = null;
JsEventInterface.BeforeSelect2AjaxInit = null;
JsEventInterface.AfterSelect2AjaxInit = null;
JsEventInterface.BeforeSelect2AjaxGrab = null;
JsEventInterface.AfterSelect2AjaxGrab = null;
JsEventInterface.BeforeInitForm = null;
JsEventInterface.AfterInitForm = null;
JsEventInterface.BeforeInitStartValue = null;
JsEventInterface.AfterInitStartValue = null;
JsEventInterface.BeforeOkReqInitStartValue = null;
JsEventInterface.AfterOkReqInitStartValue = null;
JsEventInterface.BeforeFailReqInitStartValue = null;
JsEventInterface.PageChange = null;
JsEventInterface.BeforeInitAjaxAction = null;
JsEventInterface.AfterInitAjaxAction = null;
JsEventInterface.BeforeOkReqInitAjaxAction = null;
JsEventInterface.AfterOkReqInitAjaxAction = null;
JsEventInterface.BeforeFailReqInitAjaxAction = null;

function CheckAllTable(b) {
    $('[tableCheck]').each(function () {
 
        $(this).attr('checked', b);
        sendCheckboxToRecord($(this)[0]);
    });


}
function sendCheckboxToRecord(obj) {

    angular.element(obj).scope().record.selected = obj.checked;
}


lastRecord = null;

function MergeNow(v) {
    console.log(JSON.stringify(v));
    var keys = Object.keys(v[0]);
  
    for (var i = 0; i < keys.length; i++) {
  
        if ((keys[i] != "rndId") && (keys[i] != "selected") && (keys[i] != "RowState")) {
            
            lastRecord[keys[i]] = v[0][keys[i]];
        }
    }
    currentScope.$apply({});
    NormalResult();
}
function GoValuePage(e, st, link) {
 
    if (e.keyCode == 13) {
        var d = document.createElement('a');
        d.href = '#/' + link + '/' + st;
  
        document.body.appendChild(d);
        d.click();
        document.body.removeChild(d);
    }
}
function ExportXls(id) {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = $('.table-bordered')[0]; // id of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params
    tab_text = tab_text.replace(/V/gi, "");
    tab_text = tab_text.replace(/^/gi, "");
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else                 //other browser not tested on IE 11
    {
        var a = document.createElement('a');
        a.innerHTML = "Click here";
        a.href = 'data:application/vnd.ms-excel,' + encodeURIComponent(tab_text);
        a.target = '_blank';
        a.download = window.pageName +'.xls';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        download(window.pageName,tab_text,'data:application/vnd.ms-excel')

    }
}



 
function RecalcScopes() {
    currentScope.$apply(function () { });
    if (dlgScope != null) {
        dlgScope.$apply(function () { });
    }
}

function SetupDlgScope() {

    (Object.getOwnPropertyNames(currentScope).filter(function (p) {
        var s= (typeof currentScope[p] === 'function') ;
        if(s==true)
            {
            dlgScope[p] = currentScope[p];

        }
    }));

}
function LogOut() {
    var a = document.createElement('a');
    a.innerHTML = "Click here";
    a.href = "/home/LogOut";
 
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);

}
var TempRecords = null;
var tit = null;
var Dic = new Array();
function StoreCache() {
  


}
function GenStyleForTableResponse() {
    var d = document.getElementsByTagName('table');
    if (d.length == 0) {
        return;

    }
    var th = d[0].getElementsByTagName('th');
    var r = '';
    for (var l = 0; l < th.length; l++) {
        r+='td:nth-of-type(' + (l+1).toString() +'):before { content: "' + th[l].innerText +'"; }'

    }
 
}

function PovitTableMake(datas, row, Column, value) {
    var rows = new Array();
    var columns = new Array();
    var realData = new Array();
    var sumValues = new Array();
    for (var i = 0; i < datas.length; i++) {
        var r = datas[i][row];
        var rindex = rows.indexOf(r);
        if (rindex == -1) {
            rows.push(r);
            var o = new Object();
            o.title = r;
            o.sumValue = 0;
            realData.push(o);
            rindex = realData.length - 1;
        }
        var c = datas[i][Column];
        var cindex = columns.indexOf(c);
        if (cindex == -1) {
            columns.push(c);
            sumValues.push(0);
            cindex = columns.length - 1;
        }
        realData[rindex]['col' + cindex] = (realData[rindex]['col' + cindex] === undefined ? parseFloat(datas[i][value]) : realData[rindex]['col' + cindex] + datas[i][value]);
        realData[rindex]['sumValue'] +=  parseFloat( datas[i][value] )
        sumValues[cindex] += parseFloat(datas[i][value]);

    }
    var sumO = new Object();
    sumO.title = 'جمع';
    
    for (var k = 0; k < columns.length; k++) {
        sumO['col' + k] = sumValues[k];
      
    }
    realData.push(sumO);
    var returnObject = new Object();
    returnObject.columns = columns;
    returnObject.RealDatas = realData;
    return returnObject;
}

function ExportCsv() {
    // Select rows from table_id
    var rows = document.getElementsByClassName('table-bordered')[0];
    // Construct csv
    var csv = [];
    for (var i = 0; i < rows.length; i++) {
        var row = [], cols = rows[i].querySelectorAll('td, th');
        for (var j = 0; j < cols.length; j++) {
            // Clean innertext to remove multiple spaces and jumpline (break csv)
            var data = cols[j].innerText.replace(/(\r\n|\n|\r)/gm, '').replace(/(\s\s)/gm, ' ')
            // Escape double-quote with double-double-quote (see https://stackoverflow.com/questions/17808511/properly-escape-a-double-quote-in-csv)
            data = data.replace(/"/g, '""');
            // Push escaped string
            row.push('"' + data + '"');
        }
        csv.push(row.join(';'));
    }
    var csv_string = csv.join('\n');
    // Download it
    var filename = 'export_'  + '_' + new Date().toLocaleDateString() + '.csv';
    var link = document.createElement('a');
    link.style.display = 'none';
    link.setAttribute('target', '_blank');
    link.setAttribute('href', 'data:text/csv;charset=utf-8,' + encodeURIComponent(csv_string));
    link.setAttribute('download', filename);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}


var EntitySerach = new Array();

(function (window, undefined) {

    var wd = {};

    // Default config
    wd.config = {
        database: undefined,
        table: undefined, // undefined to export all tables
        version: '1.0',
        info: '',
        dbsize: 5 * 1024 * 1024, // 5MB
        linebreaks: false,
        schemaonly: false,
        dataonly: false,
        success: function (sql) {
            console.log(sql);
        },
        error: function (message) {
            throw new Error(message);
        }
    }

    wd.exportTable = function (config) {
        // Use closure to avoid overwrites
        var table = config.table;
        // Export schema
        if (!config.dataonly) {
            wd.execute({
                db: config.db,
                sql: "SELECT sql FROM sqlite_master WHERE type='table' AND tbl_name=?;",
                params: [table],
                success: function (results) {
                    if (!results.rows || !results.rows.length) {
                        if (typeof (config.error) === "function") config.error('No such table: ' + table);
                        return;
                    }
                    config.exportSql.push('DROP TABLE [' + table +'];' + results.rows.item(0)["sql"]);
                    if (config.schemaonly) {
                        if (typeof (config.success) === "function") config.success(config.exportSql.toString());
                        return;
                    }
                }
            });
        }
        // Export data
        if (!config.schemaonly) {
            wd.execute({
                db: config.db,
                sql: "SELECT * FROM '" + table + "';",
                success: function (results) {
                    if (results.rows && results.rows.length) {
                        for (var i = 0; i < results.rows.length; i++) {
                            var row = results.rows.item(i);
                            var _fields = [];
                            var _values = [];
                            for (var col in row) {
                                _fields.push(col);
                                _values.push('"' + row[col] + '"');
                            }
                            config.exportSql.push("INSERT INTO " + table + "(" + _fields.join(",") + ") VALUES (" + _values.join(",") + ")");
                        }
                    }
                    if (typeof (config.success) === "function") config.success(config.exportSql.toString());
                },
                error: function (err) {
                    if (typeof (config.error) === "function") config.error(err);
                }
            });
        }
    }

    wd.export = function (config) {
        // Apply defaults
        for (var prop in wd.config) {
            if (typeof config[prop] === 'undefined') config[prop] = wd.config[prop];
        }
        config.db = wd.open(config);
        config.exportSql = config.exportSql || [];
        config.exportSql.toString = function () { return this.join(config.linebreaks ? ';\n' : '; ') + ';'; }
        if (config.table) {
            wd.exportTable(config);
        } else {
            config.exported = []; // list of exported tables
            config.outstanding = []; // list of outstanding tables
            var success = config.success; config.success = function () {
                config.exported.push(config.table);
                // Check if its all done
                if (config.exported.length >= config.outstanding.length) {
                    if (typeof (success) === "function") success(config.exportSql.toString());
                }
            }
            // Export all tables in db
            wd.execute({
                db: config.db,
                sql: "SELECT tbl_name FROM sqlite_master WHERE type='table';",
                success: function (results) {
                    if (results.rows) {
                        // First count the outstanding tables
                        var tbl_name;
                        for (var i = 0; i < results.rows.length; i++) {
                            tbl_name = results.rows.item(i)["tbl_name"];
                            if (tbl_name.indexOf('__WebKit') !== 0) // skip webkit internals
                                config.outstanding.push(tbl_name);
                        }
                        // Then export them
                        for (var i = 0; i < config.outstanding.length; i++) {
                            config.table = config.outstanding[i];
                            wd.exportTable(config);
                        }
                    }
                },
                error: function (err) {
                    if (typeof (error) === "function") error(transaction, err);
                }
            });
        }
    };

    wd.open = function (config) {
        if (!config) throw new Error('Please use a config object');
        if (!config.database) throw new Error('Please define a config database name.');
        return window.openDatabase(config.database, config.version || '1.0', config.info || '', config.dbsize || 512000);
    };

    // Helper method for executing SQL code in DB
    wd.execute = function (config) {
        if (!config) throw new Error('Please use a config object');
        if (!config.db) throw new Error('Please define a db obj to execute against');
        if (!config.sql) throw new Error('Please define some sql to execute.');
        config.db.transaction(function (transaction) {
            transaction.executeSql(config.sql, config.params || [],
                function (transaction, results) {
                    if (typeof (config.success) === "function") config.success(results);
                },
                function (transaction, err) {
                    if (typeof (config.error) === "function") config.error(err);
                }
            );
        });
    };

    window.websqldump = {
        export: function () {
            wd.export.apply(wd, arguments);
        }
    };

})(this);
 
function GetBackupDownload() {
    websqldump.export({
        database: dbname, version: "1", success: function (sql) {

            var filename = 'export_' + '_' + new Date().toLocaleDateString() + '.sql';
            var link = document.createElement('a');
            link.style.display = 'none';
            link.setAttribute('target', '_blank');
            link.setAttribute('href', 'data:text/sql;charset=utf-8,' + encodeURIComponent(sql));
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);



        }, error: function (message) {
            throw new Error(message);
        }
    });


}

function RestoreBackup() {
    var d = document.createElement("input");
    d.type = "file";
    d.onchange = function (e) {
        if (e.target.files.length > 0) {
            if (e.target.files[0].name.indexOf('.sql') != -1) {
                var Filer = new FileReader();
                Filer.onload = function (e) {
                    var data = e.target.result;
                    
                    var coms = data.split(';');
                    db.transaction(function (tx) {
                        for (var z = 0; z < coms.length-1; z++) {

                            tx.executeSql(coms[z], [], function (tx, results) { });
                        }

                      
                    }, function (e, x) { alert(e.message); }, function (a, b) { Messager.ShowMessage('بازیابی ','با موفقیت انجام شد')});

                }
                Filer.readAsText(e.target.files[0]);
                 
            }

        }


    }
    d.click();


}function GetBackupDownload() {
    websqldump.export({
        database: dbname, version: "1", success: function (sql) {

            var filename = 'export_' + '_' + new Date().toLocaleDateString() + '.sql';
            var link = document.createElement('a');
            link.style.display = 'none';
            link.setAttribute('target', '_blank');
            link.setAttribute('href', 'data:text/sql;charset=utf-8,' + encodeURIComponent(sql));
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);



        }, error: function (message) {
            throw new Error(message);
        }
    });


}

function RestoreBackup() {
    var d = document.createElement("input");
    d.type = "file";
    d.onchange = function (e) {
        if (e.target.files.length > 0) {
            if (e.target.files[0].name.indexOf('.sql') != -1) {
                var Filer = new FileReader();
                Filer.onload = function (e) {
                    var data = e.target.result;
                    
                    var coms = data.split(';');
                    db.transaction(function (tx) {
                        for (var z = 0; z < coms.length-1; z++) {

                            tx.executeSql(coms[z], [], function (tx, results) { });
                        }

                      
                    }, function (e, x) { alert(e.message); }, function (a, b) { Messager.ShowMessage('بازیابی ','با موفقیت انجام شد')});

                }
                Filer.readAsText(e.target.files[0]);
                 
            }

        }


    }
    d.click();


}



 