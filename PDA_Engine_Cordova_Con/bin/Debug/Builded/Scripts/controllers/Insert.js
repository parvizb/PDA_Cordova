/// <reference path="../../Res/toolkit.js" />



var Insert=new Object();

var currentButton;
Insert.sendFiles=  function()
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
            Insert.Submit(currentButton);
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
    xhr.open('POST', "Home/SendFiles?PageName=Insert");
    // xhr.setRequestHeader("Content-type", "multipart/form-data");
    xhr.send(data);
}





Insert.DownloadNow= function (obj)
{
    currentButton=obj;
    $(obj).attr('disabled',true);
    if(Insert.Validate()==false)
    {
        $(obj).attr('disabled',false);
        return ;
    }
 
    var Entity=new Object();
    Entity.PageName='Insert';
    Entity.Parameters=new Array();
                Entity.Parameters.push( toInput('fullname',$('#txtInsertfullname').val()));
    
                    Entity.Parameters.push( toInput('cost',$('#txtInsertcost').val()));
    
                    Entity.Parameters.push( toInput('BrithDate',$('#txtInsertBrithDate').val()));
    
                    Entity.Parameters.push( toInput('isbad', $('#txtInsertisbad').val()) );
    
                    Entity.Parameters.push( toInput('IdAbout',$('#txtInsertIdAbout').val()));
    
        
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

Insert.Submit= function(obj)
{
    currentButton=obj;
    $(obj).attr('disabled',true);
    if(Insert.Validate()==false)
    {
        $(obj).attr('disabled',false);
        return ;
    }
        var Entity=new Object();
    Entity.PageName='Insert';
    Entity.Parameters=new Array();
            p='String';
        Entity.Parameters.push( toInput('fullname',$('#txtInsertfullname').val()));
    
                p='Money';
        Entity.Parameters.push( toInput('cost',$('#txtInsertcost').val() ,false) );
    
                p='Date';
        Entity.Parameters.push( toInput('BrithDate',  NorDate($('#txtInsertBrithDate').val())  ) );
    
    
  
    
                p='CheckBox';
        Entity.Parameters.push( toInput('isbad',ConvertBit( $('#txtInsertisbad').val() ) ,false) );
    
                p='Select2Ajax';
        Entity.Parameters.push( toInput('IdAbout',$('#txtInsertIdAbout').val()));
    
        

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
Insert.Validate= function()
{
    Validator.ClearErrors();
        
                                Validator.CheckEmpty('txtInsertfullname','نام');
                                                                                                    
            
                                                                                Validator.CheckRegDate('txtInsertBrithDate','تاریخ');
                                    
                                                                                    
        
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


Insert.Serach=function(obj,dataP)
{
    $(obj).attr('disabled',true);
    if(dataP==null){
        if(Insert.Validate()==false)
        {
            $(obj).attr('disabled',false);
            return ;
        }
    }
    window.CurrentSerachMethod=Insert.Serach;
    var Entity=new Object();
    if(dataP===undefined){
    Entity.PageName='Insert';
    Entity.Parameters=new Array();
                Entity.Parameters.push( toInput('fullname',$('#txtInsertfullname').val()));
    
                    Entity.Parameters.push( toInput('cost',$('#txtInsertcost').val() ,false) );
    
                    Entity.Parameters.push( toInput('BrithDate',  NorDate($('#txtInsertBrithDate').val())  ) );
    
                    Entity.Parameters.push( toInput('isbad',  ConvertBit($('#txtInsertisbad').val())  ) );
    
  
    
                    Entity.Parameters.push( toInput('IdAbout',$('#txtInsertIdAbout').val()));
    
        }


p=''

 

p='Static'
SetDefaultValue(Entity.Parameters,'cost', 0);

 

p=''

 

p=''

 

p=''

 

TableViewAjax('getTableViewRecords',(dataP!==undefined?dataP: Entity),function(data){
          
    currentScope.Insertrecords= data.records;
        totalRecords= data.RecordTotal;
    GenPagingLinks();
        setTimeout(StoreCache, 200);
    currentScope.$apply(function(){});
    if(dlgScope!=null)
    {
        dlgScope.Insertrecords= data.records;
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




