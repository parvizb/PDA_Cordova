function DBAction(pageName, method) {
    {% for pages in App.Pagess  -%}
    {% for Page in pages.Pages  -%}
    if(pageName =='{{Page.Name}}') {
        if(method == 'ScallerSubmit'){
            var r= "{{Page.DBCommand}}";

            return r.substring(1,r.length);
        }
        if(method == 'getTableViewRecords'){
            var r= "{{Page.DBCommand}}";

            return r.substring(1,r.length);
        }
        if(method =='getStartValueFromServer')
        {
            var r="{{Page.ValueDbCommand}}";

            return r.substring(1,r.length);
        }
        
    }
    {% for para in Page.PageParameters-%}
    {% if para.DBSelect2Command !='' -%}
    if (pageName =='{{Page.Name}}@{{para.name}}'){
        var r="{{para.DBSelect2Command}}";

        return r.substring(1,r.length);

    }
    {% endif -%}
    {% if para.DBSelect2Command !='' -%}
    if (pageName =='{{Page.Name}}@{{para.name}}_direct'){
        var r="{{para.DBSelect2CommandDriectValue}}";

        return r.substring(1,r.length);

    }
    {% endif -%}
    {% endfor -%}
   
    {% endfor -%}
    {% endfor -%}
    {% for actions in App.Actionss -%}
    {% for action in actions.actions -%}
    if (pageName=='aa_{{action.name}}')
    {
        var r="{{action.DBCommand}}";
        return r.substring(1,r.length);

    }
    
    {% endfor -%}
    {% endfor -%}
     
}
function Select2Prop(pagename,name,istable)
{
    var obj=new Object();
    obj.Command="";
    obj.options=new Array();
    obj.idcol="";
    obj.textcol="";
    obj.DirectParameterName="";
    {% for pages in App.Pagess  -%}
    {% for Page in pages.Pages  -%}
    if (pagename == '{{Page.Name}}')
    {
        {% for para in Page.PageParameters-%}
        {% if para.DBSelect2Command !='' %}

        if (name=='{{para.name}}'   && istable==false)
        {
            obj.Command="{{para.DBSelect2Command}}".substring(1);
            obj.idcol='{{para.codeColumn}}';
            obj.textcol='{{para.textColumn}}';
            obj.DirectParameterName='{{para.DBSelect2CommandDriectValueParameterName}}';
            {% for op in para.options %}
            obj.options.push( { id:"{{op.value}}",text="{{op.text}}"  });
            {% endfor -%}
            return obj;
        }
        {% endif -%}
        {% endfor -%}

        {% for tab in Page.tables %}
        {% for col in tab.columns %}
        {% if col.DBSelect2Command != '' %}
        if (name=='{{col.name}}'   && istable==true)
        {
            obj.Command="{{col.DBSelect2Command}}".substring(1);
            obj.idcol='{{col.codeColumn}}';
            obj.textcol='{{col.textColumn}}';
            {% for op in col.options %}
            obj.options.push( { id:"{{op.value}}",text="{{op.text}}"  });
            {% endfor -%}
            return obj;
        } 

        {% endif -%}

        {% endfor -%}
        {% endfor -%}
    }
    {% endfor -%}
    {% endfor -%}
}


