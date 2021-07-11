function timeToMin()
{
    var d = new Date();
  var n = d.getTime();
  n/=(1000*60);
  return  Math.round( n);

}


function SetInF(parameters,fnOk,fnFail)
{
  alert("salam");
  var da = new Object(); da.Message = 'با موفقیت انجام شد'; da.code = 0;
  da.retrunValue = null;
   
  fnOk(da);

}

function SetOutFunView(parameters,fnOk,fnFail)
{
   ReadTable("Select * from Unit,Setup where IdUnit=@IdUnit",parameters,function( x ) {  
    x=x;
    obj=JSON.parse(x.retrunValue).Records[0];
    $('#txtSetOutIdNumber').val(obj.IdNumber);
    var x2=obj.pin;
    var y=timeToMin();
    y-=x2;
 
    $('#txtSetOutHour').val(Math.round( y/60)   );
    $('#txtSetOutPrice').val(obj.PerHour);
    $('#txtSetOutEnterPrice').val(obj.Enter);
    fnOk(x);
    RecalcScopes();
   }  ,function(y){} );

  
}

function SaveOut(parameters,fnOk,fnFail)
{
  Scaller("Insert into Log(IdUnit,IdNumber,dDate,TotalPrice) values (@IdUnit,@IdNumber,@dDate,@TotalPrice) " ,parameters,
   function(x) {
      
      Scaller("Update Unit set IdNumber=null,pin=null where IdUnit=@IdUnit",parameters, fnOk,fnFail );
   },
   function(y){


   }
  
  
  );



}