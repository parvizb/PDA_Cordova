var tempVal=$('#txt{{Page.Name}}{{PageParameter.name}}').val();
if(tempVal.length==0)
{
    Messager.errors.push('{{PageParameter.title}} انتخاب نشده   ردیفی در کادر ');
}
