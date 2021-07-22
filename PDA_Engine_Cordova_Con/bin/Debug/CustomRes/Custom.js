function GetCustomDataF(para,fnOk,fn)
{
  var da=new Object();
  var re={"Records":[ {"name":"parviz","age":"10"},{"name":"ali","age":"20"}]};
  
  var da = new Object(); da.Message = 'با موفقیت انجام شد'; da.code = 0;
  da.retrunValue=JSON.stringify(re);
  fnOk(da);

}

function GetCustomDataS(para,fnOk,fn)
{
  var da=new Object();
   
  
  var da = new Object(); da.Message = 'با موفقیت انجام شد'; da.code = 0;
  da.retrunValue=para[0].value*2;
  alert('salam ' +  da.retrunValue );
  fnOk(da);

}

function MakeRandomData(para,fnOk,fn)
{
  var da=new Object();
  var re=new Object();
  re.Records=new Array();
  for(var i=0;i<100;i++)
  {
   var x={"a":Math.round(Math.random()*4),"b":Math.round(Math.random()*4),"c":Math.round(Math.random()*100)};
   re.Records.push(x);
  }
  
  var da = new Object(); da.Message = 'با موفقیت انجام شد'; da.code = 0;
  da.retrunValue=JSON.stringify(re);
  fnOk(da);



}