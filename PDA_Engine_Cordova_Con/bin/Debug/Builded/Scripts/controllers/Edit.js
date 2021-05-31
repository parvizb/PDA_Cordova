/// <reference path="../../Res/toolkit.js" />



var Edit=new Object();

var currentButton;
Edit.sendFiles=  function()
{
    var data = new FormData();
 
                                                $('#loadingBar').show();
    $('#fileStatus').show();
    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", function (evt) {
        if (evt.lengthComputable) {
            var progress = Math.round(evt.loaded * 100 / evt.total);
            $('#progessStatus').css('width'  , progress + '%');
            $('#progessStatus').attr('aria-valuenow'  , progress *2);
        }
    }, false);

    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200 ) {
            window.fileUploaded=true;
            $('#loadingBar').hide();
            $('#fileStatus').hide();
            Edit.Submit(currentButton);
        } else {
            if((xhr.status==500) && (xhr.readyState == 4))
            {
                $('#fileStatus').hide();
                $('#loadingBar').hide();
                var dos=document.createElement("s");
                dos.innerHTML=xhr.responseText;
                 
                alert("خطا در ارسال فایل ها" +  dos.getElementsByTagName("title")[0].innerHTML );
                    
            }
         
        }
    };
    xhr.open('POST', "Home/SendFiles?PageName=Edit");
    // xhr.setRequestHeader("Content-type", "multipart/form-data");
    xhr.send(data);
}





Edit.DownloadNow= function (obj)
{
    currentButton=obj;
    $(obj).attr('disabled',true);
    if(Edit.Validate()==false)
    {
        $(obj).attr('disabled',false);
        return ;
    }
 
    var Entity=new Object();
    Entity.PageName='Edit';
    Entity.Parameters=new Array();
                Entity.Parameters.push( toInput('fullname',$('#txtEditfullname').val()));
    
                    Entity.Parameters.push( toInput('cost',$('#txtEditcost').val()));
    
                    Entity.Parameters.push( toInput('BrithDate',$('#txtEditBrithDate').val()));
    
                    Entity.Parameters.push( toInput('isbad', $('#txtEditisbad').val()) );
    
                    Entity.Parameters.push( toInput('Id',routeParams.Id ));

var urlx="/Home/DownloadNow?PageName="  + Entity.PageName + "&Params=";
if(Entity.Parameters.length!=0)
{
    for(var i=0;i<Entity.Parameters.length;i++)
    {
        urlx+=Entity.Parameters[i].key + "|" + Entity.Parameters[i].value +"|";

    }

}

window.open(urlx);
 
$(obj).attr('disabled',false);
}

Edit.Submit= function(obj)
{
    currentButton=obj;
    $(obj).attr('disabled',true);
    if(Edit.Validate()==false)
    {
        $(obj).attr('disabled',false);
        return ;
    }
        var Entity=new Object();
    Entity.PageName='Edit';
    Entity.Parameters=new Array();
            p='String';
        Entity.Parameters.push( toInput('fullname',$('#txtEditfullname').val()));
    
                p='Money';
        Entity.Parameters.push( toInput('cost',$('#txtEditcost').val() ,false) );
    
                p='Date';
        Entity.Parameters.push( toInput('BrithDate',  NorDate($('#txtEditBrithDate').val())  ) );
    
    
  
    
                p='CheckBox';
        Entity.Parameters.push( toInput('isbad',ConvertBit( $('#txtEditisbad').val() ) ,false) );
    
                    Entity.Parameters.push( toInput('Id',routeParams.Id ));


p=''

 

p='Static'
SetDefaultValue(Entity.Parameters,'cost', 0);

 

p=''

 

p=''

 

p=''

 



ScallerAjax('ScallerSubmit',Entity,function(data){
        Messager.ShowMessage('اطلاعات', data.Message  );
        
  
 

  
    if(JsEventInterface.AfterOkReqSubmit!=null)
    {
        JsEventInterface.AfterOkReqSubmit(Entity,data);
    }
 
                    BackPage();
         
     
     
  


    $(obj).attr('disabled',false);
    return;
       
},function(data)
{
    $(obj).attr('disabled',false);
    return;

});
};
Edit.Validate= function()
{
    Validator.ClearErrors();
        
                                Validator.CheckEmpty('txtEditfullname','نام');
                                                                                                    
            
                                                                                Validator.CheckRegDate('txtEditBrithDate','تاریخ');
                                    
                                                                                    
        
    if(Messager.errors.length!=0)
    {
        Validator.ShowErrors();
        return false ;
    }
    
    if(Messager.errors.length!=0)
    {

        Validator.ShowErrors();
        return false ;
    }


    return Messager.errors.length==0;
}


Edit.Serach=function(obj,dataP)
{
    $(obj).attr('disabled',true);
    if(dataP==null){
        if(Edit.Validate()==false)
        {
            $(obj).attr('disabled',false);
            return ;
        }
    }
    window.CurrentSerachMethod=Edit.Serach;
    var Entity=new Object();
    if(dataP===undefined){
    Entity.PageName='Edit';
    Entity.Parameters=new Array();
                Entity.Parameters.push( toInput('fullname',$('#txtEditfullname').val()));
    
                    Entity.Parameters.push( toInput('cost',$('#txtEditcost').val() ,false) );
    
                    Entity.Parameters.push( toInput('BrithDate',  NorDate($('#txtEditBrithDate').val())  ) );
    
                    Entity.Parameters.push( toInput('isbad',  ConvertBit($('#txtEditisbad').val())  ) );
    
  
    
                    Entity.Parameters.push( toInput('Id',routeParams.Id ));
}


p=''

 

p='Static'
SetDefaultValue(Entity.Parameters,'cost', 0);

 

p=''

 

p=''

 

p=''

 

TableViewAjax('getTableViewRecords',(dataP!==undefined?dataP: Entity),function(data){
          
    currentScope.Editrecords= data.records;
        totalRecords= data.RecordTotal;
    GenPagingLinks();
        setTimeout(StoreCache, 200);
    currentScope.$apply(function(){});
    if(dlgScope!=null)
    {
        dlgScope.Editrecords= data.records;
                dlgScope.$apply(function(){});

    }
    $('[type="Select2Ajax"]').each(function(){
    $(this).val($(this).attr('valc'));

});
NormalResult();
        
$(obj).attr('disabled',false);
return;
          
},function(data)
{
    $(obj).attr('disabled',false);
    return;

});


}




window.targetElement=null;
Edit.InitStartValues=function(){
    var Entity=new Object();
    Entity.PageName='Edit';
    Entity.Parameters=new Array();
            Entity.Parameters.push( toInput('Id',routeParams.Id));
   
TableViewAjax('getStartValueFromServer',Entity,function(data){
    if( data.records.length!=0)
    {
     
                                
$('#txtEditfullname').val(data.records[0].fullname);

                        
$('#txtEditcost').val(ShowAsMoney( data.records[0].cost));

                        }
else
{
    Validator.ClearErrors ();
    Messager.ShowMessage('خطا', 'رکورد مورد نظر یافت نشد');
    BackPage();
}
                
currentScope.$apply();
return;
          
},function(data)
{
    return;

});

}




