using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
namespace PDA_Engine_Cordova_Con
{
    public static class XmlValidator
    {
        public static Dictionary<XmlNode, string> err = new Dictionary<XmlNode, string>();
        public static List<inputParameter> toInputParameter(this List<PageParameter> Ps)
        {
            List<inputParameter> ps = new List<inputParameter>();
            foreach (PageParameter p in Ps)
            {
                ps.Add(new inputParameter(p.name, "0"));

            }

            return ps;
        }
        public static string output()
        {
            string r = "";
            foreach(XmlNode n in err.Keys)
            {
                r += n.OuterXml + "\r\n" + err[n];
                r += "\r\n*****";


            }

            return r;
        }

        public static List<inputParameter> toInputParameter(this List<ValueParameter> Ps)
        {
            List<inputParameter> ps = new List<inputParameter>();
            foreach (ValueParameter p in Ps)
            {
                ps.Add(new inputParameter(p.name, "0"));

            }

            return ps;
        }
        public static List<inputParameter> toInputParameter(this List<Parameter> Ps)
        {
            List<inputParameter> ps = new List<inputParameter>();
            foreach (Parameter p in Ps)
            {
                ps.Add(new inputParameter(p.name, "0"));

            }

            return ps;
        }

        public static List<inputParameter> toInputParameter(this List<DBSelectCommandParameter> Ps)
        {
            List<inputParameter> ps = new List<inputParameter>();
            foreach (DBSelectCommandParameter p in Ps)
            {
                ps.Add(new inputParameter(p.name, "0"));

            }

            return ps;
        }
        public static List<inputParameter> toInputParameter(this List<actionParameter> Ps)
        {
            List<inputParameter> ps = new List<inputParameter>();
            foreach (actionParameter p in Ps)
            {
                ps.Add(new inputParameter(p.name, "0"));

            }

            return ps;
        }
    }

    public class xml : ILiquidizable
    {
        public string version;
        public string encoding;
        public void ParseEle(XmlNode node)
        {

            this.version = node.Attr("version");
            this.encoding = node.Attr("encoding");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { version = this.version, encoding = this.encoding });

        }

    }


    public static class ValidatorChecker
    {
        public static void CheckNotEmpty(XmlNode Xm, string val, string[] states, string[] props)
        {
            string pr = "";
            if (Array.IndexOf(states, val) != -1)
            {
                for (int k = 0; k < props.Length; k++)
                {
                    if (Xm.Attr(props[k]) == "")
                    {
                        pr = "\r\nمشخصه " + props[k] + " نباید خالی باشد";

                    }
                }

            }
            if (pr != "")
            {
                bool found = false;
                string ez = "";
                found = XmlValidator.err.TryGetValue(Xm, out ez);
                if (found == false)
                {
                    XmlValidator.err.Add(Xm, pr);
                }
                else
                {
                    XmlValidator.err[Xm] = ez + pr;

                }
            }
        }
        public static void CheckItemCount(XmlNode Xm, string val, string[] states, int itms, string title)
        {
            string pr = "";
            if (Array.IndexOf(states, val) != -1)
            {
                if (itms == 0)
                {
                    pr = "\r\n هیچ موردی به عنوان  " + title + " تعین نشده ";
                }

            }
            if (pr != "")
            {
                bool found = false;
                string ez = "";
                found = XmlValidator.err.TryGetValue(Xm, out ez);
                if (found == false)
                {
                    XmlValidator.err.Add(Xm, pr);
                }
                else
                {
                    XmlValidator.err[Xm] = ez + pr;

                }
            }
        }
        public static void CheckItemCount(XmlNode Xm, int itms, string title)
        {
            string pr = "";
            if (itms == 0)
            {
                pr = "\r\n هیچ موردی به عنوان  " + title + " تعین نشده ";
            }




            if (pr != "")
            {
                bool found = false;
                string ez = "";
                found = XmlValidator.err.TryGetValue(Xm, out ez);
                if (found == false)
                {
                    XmlValidator.err.Add(Xm, pr);
                }
                else
                {
                    XmlValidator.err[Xm] = ez + pr;

                }
            }
        }
    }


    /// <summary>
    /// کلاس اصلی نرم افزار
    /// </summary>
    public class App : ILiquidizable
    {
        public XmlNode RootNode = null;
        /// <summary>
        /// عنوان برنامه مشخص می کند
        /// </summary>
        public string Title;
        /// <summary>
        /// نسخه برنامه را مشخص می کند
        /// </summary>
        public string Version;
        /// <summary>
        /// نام انگلیسی برنامه را مشخص می کند
        /// </summary>
        public string Name;
        public string Debug;
        public string PreventCache;

        public string ConnectionSetthing;
        /// <summary>
        /// تم را مشخص می کند
        /// </summary>
        public string Theme;
        /// <summary>
        /// 
        public string CopyRightMessage;
        /// منو های 
        /// </summary>
        public List<Menu> Menus = new List<Menu>();
        /// <summary>
        /// صفحات
        /// </summary>
        public List<Pages> Pagess = new List<Pages>();
        /// <summary>
        /// متد مورد نظر برای Ajax را مشخص می کند
        /// </summary>
        public List<Actions> Actionss = new List<Actions>();
        public List<DontBuild> DontBuilds = new List<DontBuild>();
        public List<Notifaction> Notifactions = new List<Notifaction>();
        public List<PrecreatedData> PrecreatedDatas = new List<PrecreatedData>();
        public List<DataBaseTable> DataBaseTables = new List<DataBaseTable>();
        public List<StoredProc> StoredProcs = new List<StoredProc>();
        public void ParseEle(XmlNode node)
        {


            PDAL.App = this;
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }



            this.Debug = node.Attr("Debug");
            this.Title = node.Attr("Title");
            this.Version = node.Attr("Version");
            this.Name = node.Attr("Name");
            this.CopyRightMessage = node.Attr("CopyRightMessage");
            this.ConnectionSetthing = node.Attr("ConnectionSetthing");
            this.PreventCache = node.Attr("PreventCache");
            this.Theme = node.Attr("Theme");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Menu")
                {
                    Menu Temp = new Menu();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Menus.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Notifaction")
                {
                    Notifaction Temp = new Notifaction();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Notifactions.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "StoredProc")
                {
                    StoredProc Temp = new StoredProc();
                    Temp.ParseEle(node.ChildNodes[i]);
                    StoredProcs.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Pages")
                {
                    Pages Temp = new Pages();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Pagess.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Var")
                {
                    PDAL.Vars.Add(node.ChildNodes[i].Attr("Name"), node.ChildNodes[i].Attr("Value"));



                }
                if (node.ChildNodes[i].Name == "DontBuild")
                {
                    DontBuild Temp = new DontBuild();
                    Temp.ParseEle(node.ChildNodes[i]);
                    DontBuilds.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Actions")
                {
                    Actions Temp = new Actions();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Actionss.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "DataBaseTable")
                {
                    DataBaseTable Temp = new DataBaseTable();
                    Temp.ParseEle(node.ChildNodes[i]);
                    DataBaseTables.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "PrecreatedData")
                {
                    PrecreatedData Temp = new PrecreatedData();
                    Temp.ParseEle(node.ChildNodes[i]);
                    PrecreatedDatas.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
                if (node.ChildNodes[i].Name == "Include")
                {
                    XmlNode xn = node.ChildNodes[i];
                    XmlDocument XmlDoc2 = new XmlDocument();
                    XmlDoc2.LoadXml(System.IO.File.ReadAllText(PDAL.MapPath(xn.Attr("FilePath"))));
                    XmlNode el = PDAL.GetElement(XmlDoc2, "Application");
                    App Ap = new App();
                    Ap.ParseEle(el);
                    if (xn.Attr("ImportMenuItems") == "Yes")
                    {
                        this.Menus[0].Items.AddRange(Ap.Menus[0].Items);
                    }
                    if (xn.Attr("ImportPages") == "Yes")
                    {
                        for (int k = 0; k < Ap.Pagess.Count; k++)
                        {
                            for (int l = 0; l < Ap.Pagess[k]._Pages.Count; l++)
                            {
                                this.Pagess[0]._Pages.Add(Ap.Pagess[k]._Pages[l]);
                                PDAL.PageAccess.Add(Ap.Pagess[k]._Pages[l].name, PDAL.App.Pagess[0]._Pages[PDAL.App.Pagess[0]._Pages.Count - 1]);
                            }

                        }
                    }
                    if (xn.Attr("ImportActions") == "Yes")
                    {
                        for (int k = 0; k < Ap.Actionss.Count; k++)
                        {
                            for (int l = 0; l < Ap.Actionss[k].actions.Count; l++)
                            {
                                this.Actionss[0].actions.Add(Ap.Actionss[k].actions[l]);
                                PDAL.actionAccess.Add(Ap.Actionss[k].actions[l].name, Ap.Actionss[k].actions[l]);
                            }

                        }
                    }
                    if (xn.Attr("ImportDataBaseTables") == "Yes")
                    {
                        for (int k = 0; k < Ap.DataBaseTables.Count; k++)
                        {
                            this.DataBaseTables.Add(Ap.DataBaseTables[k]);

                        }
                    }
                }
            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { PrecreatedDatas = this.PrecreatedDatas, Notifactions = this.Notifactions, CopyRightMessage = this.CopyRightMessage, Theme = this.Theme, Title = this.Title, Version = this.Version, Name = this.Name, ConnectionSetthing = this.ConnectionSetthing, Menus = this.Menus, Pagess = this.Pagess, Actionss = this.Actionss });

        }

    }
    public class PrecreatedData:ILiquidizable
    {
        public string name;
        public string url;
        public void ParseEle(XmlNode T)
        {
            name = T.Attr("name");
            url = T.Attr("url");

        }


        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new {name=this.name,url=this.url });
        }
    }

    public class StoredProc : ILiquidizable
    {
        public string name;
        public string Proc;
        public List<Column> Columns = new List<Column>();

        public void ParseEle(XmlNode T)
        {
            name = T.Attr("name");
            Proc = T.InnerXml;


        }

        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { name = this.name, Columns = this.Columns });
        }
    }


    public class DataBaseTable:ILiquidizable
    {
        public string name;
        public List<Column> Columns=new List<Column>();

        public void ParseEle(XmlNode T)
        {
            name = T.Attr("name");
            foreach (XmlNode Z in T.ChildNodes)
            {
                if (Z.Name == "Column")
                {
                    Column Temp = new Column();
                    Temp.ParseEle(Z);
                    Columns.Add(Temp);

                }
            }


        }
       
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new {name=this.name ,Columns=this.Columns });
        }
    }
    public class Column:ILiquidizable
    {
        public string Name;
        public string Caption;
        public string DataType;
        public string isPrimary;
        
        public void ParseEle(XmlNode T)
        {
            Name = T.Attr("Name");
            Caption = T.Attr("Caption");
            DataType = T.Attr("DataType");
            isPrimary = T.Attr("isPrimary");


        }
      
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Name=this.Name,Caption=this.Caption,DataType=this.DataType,isPrimary=this.isPrimary });
        }
    }

    public class BatchCommand : ILiquidizable
    {

        public string name;
        public string PerKey;
        public List<Command> Commands = new List<Command>();
        public List<CommandCustomValidate> CommandCustomValidates = new List<CommandCustomValidate>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.name = node.Attr("name");
            this.PerKey = node.Attr("PerKey");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Command")
                {
                    Command Temp = new Command();

                    Temp.ParseEle(node.ChildNodes[i]);

                    Commands.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CommandCustomValidate")
                {
                    CommandCustomValidate Temp = new CommandCustomValidate();
                    Temp.ParseEle(node.ChildNodes[i]);
                    CommandCustomValidates.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckItemCount(node, Commands.Count, "دستورات اجرایی");
            }
        }
        public object ToLiquid()
        {

            return Hash.FromAnonymousObject(new { CommandCustomValidates = this.CommandCustomValidates, PerKey = this.PerKey, name = this.name, Commands = this.Commands });

        }

    }
    public class Command : ILiquidizable
    {

        public string DBCommand;
        public string Selection;
        public string StateMode;
        public string SelectionParameter;
        public List<Parameter> Parameters = new List<Parameter>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.DBCommand = node.Attr("DBCommand");
            this.Selection = node.Attr("Selection");
            this.StateMode = node.Attr("StateMode");
            this.SelectionParameter = node.Attr("SelectionParameter");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Parameter")
                {
                    Parameter Temp = new Parameter();
                    Temp.ParseEle(node.ChildNodes[i]);

                    Parameters.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.Selection, new string[] { "EveryValueInSelect" }, new string[] { "SelectionParameter" });
            }

        }

        public object ToLiquid()
        {

            Util.InitCommanD(ref DBCommand, Parameters.toInputParameter(), null);

            return Hash.FromAnonymousObject(new { SelectionParameter = this.SelectionParameter, DBCommand = this.DBCommand, Selection = this.Selection, StateMode = this.StateMode, Parameters = this.Parameters });

        }

    }
    public class Notifaction : ILiquidizable
    {
        public string name;
        public string AjaxActionName;
        public string TimeFire;
        public string Message;
        public string PageName;
        public string ParameterSyntax;
        public string returnSyntax;
        public List<Check> Checks = new List<Check>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.name = node.Attr("name");
            this.TimeFire = node.Attr("TimeFire");
            this.AjaxActionName = node.Attr("AjaxActionName");
            PageName = node.Attr("PageName");
            ParameterSyntax = node.Attr("ParameterSyntax");
            returnSyntax = node.Attr("returnSyntax");
            this.Message = node.Attr("Message");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {


            }

        }
        public object ToLiquid()
        {
            if (AjaxActionName == null)
            {
                new Exception("fuck");

            }
            return Hash.FromAnonymousObject(new { ParameterSyntax = this.ParameterSyntax, returnSyntax = this.returnSyntax, PageName = this.PageName, TimeFire = this.TimeFire, name = this.name, AjaxActionName = this.AjaxActionName, Message = this.Message });

        }

    }




    public class Parameter : ILiquidizable
    {

        public string name;
        public string sourceType;
        public string caption;
        public string sourceTypeParameter;
        public string Disabled;
        public string DefaultValueSource;
        public string DefaultValueParameter;
        public List<Check> Checks = new List<Check>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.name = node.Attr("name");
            this.caption = node.Attr("caption");
            this.sourceType = node.Attr("sourceType");
            Disabled = node.Attr("Disabled");
            DefaultValueSource = node.Attr("DefaultValueSource");
            DefaultValueParameter = node.Attr("DefaultValueParameter");
            this.sourceTypeParameter = node.Attr("sourceTypeParameter");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Check")
                {
                    Check Temp = new Check();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Checks.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.sourceType, new string[] { "PageParameter", "QueryString", "Row", "Session", "ScallerValues", "Expr", "SpecValue" }, new string[] { "sourceTypeParameter" });
                ValidatorChecker.CheckNotEmpty(node, this.DefaultValueSource, new string[] { "PageParameter", "QueryString", "Row", "Session", "ScallerValues", "Expr", "SpecValue" }, new string[] { "DefaultValueParameter" });

            }
        }
        public string RealData = "";

        public object ToLiquid()
        {





            if (sourceType == null)
            {
                new Exception("fuck");

            }

            return Hash.FromAnonymousObject(new { DefaultValueSource = this.DefaultValueSource, DefaultValueParameter = this.DefaultValueParameter, Disabled = this.Disabled, caption = this.caption, name = this.name, sourceType = this.sourceType, sourceTypeParameter = this.sourceTypeParameter, Checks = this.Checks });

        }

    }
    public class Check : ILiquidizable
    {
        public string Type;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Type = node.Attr("Type");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Type = this.Type });

        }

    }

    /// <summary>
    ///ساختار منو
    /// </summary>
    public class Menu : ILiquidizable
    {
        /// <summary>
        /// عنوان منو را مشخص می کند 
        /// </summary>
        public string Title;
        /// <summary>
        /// منو ها را مشخص می کند
        /// </summary>
        public List<Item> Items = new List<Item>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Title = node.Attr("Title");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Item")
                {
                    Item Temp = new Item();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Items.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Title = this.Title, Items = this.Items });

        }

    }

    /// <summary>
    /// گزینه منو
    /// </summary>
    public class Item : ILiquidizable
    {
        /// <summary>
        /// نوع گزینه منو مشخص می کند
        /// </summary>
        public string Type;
        /// <summary>
        /// لینک را مشخص می کند
        /// </summary>
        public string link;
        /// <summary>
        /// عنوان نمایشی 
        /// </summary>
        public string text;
        /// <summary>
        /// مجوز مورد نیاز برای مشاهده
        /// </summary>
        public string PerKey;
        /// <summary>
        /// شناسه مورد نظر
        /// </summary>
        public string id;
        /// <summary>
        /// متن توضیحی برای گزینه های TextBox مشخص می کند
        /// </summary>
        public string placeholder;
        /// <summary>
        /// زیر مجموعه ها را مشخص می کند
        /// </summary>
        public List<SubItem> SubItems = new List<SubItem>();


        public string Icon = "";
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Type = node.Attr("Type");
            this.link = node.Attr("link");
            this.text = node.Attr("text");
            this.PerKey = node.Attr("PerKey");
            this.Icon = node.Attr("Icon");
            this.id = node.Attr("id");
            this.placeholder = node.Attr("placeholder");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "SubItem")
                {
                    SubItem Temp = new SubItem();
                    Temp.ParseEle(node.ChildNodes[i]);
                    SubItems.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.Type, new string[] { "PDALink", "PDATextboxLink", "JavaScript" }, new string[] { "link" });


            }
        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Icon = this.Icon, Type = this.Type, link = this.link, text = this.text, PerKey = this.PerKey, id = this.id, placeholder = this.placeholder, SubItems = this.SubItems });

        }

    }
    /// <summary>
    /// گزینه زیر منو
    /// </summary>
    public class SubItem : ILiquidizable
    {
        /// <summary>
        /// نوع آنرا مشخص می کند 
        /// </summary>
        public string Type;
        /// <summary>
        /// آدرس مورد نظر
        /// </summary>
        public string link;
        /// <summary>
        /// مجوز مورد نیاز برای مشاهده 
        /// </summary>
        public string PerKey;
        /// <summary>
        /// عنوان
        /// </summary>
        public string text;

        public string Icon;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Type = node.Attr("Type");
            this.link = node.Attr("link");
            this.PerKey = node.Attr("PerKey");
            this.text = node.Attr("text");
            this.Icon = node.Attr("Icon");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.Type, new string[] { "PDALink", "PDATextboxLink", "JavaScript" }, new string[] { "link" });


            }
        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Icon = this.Icon, Type = this.Type, link = this.link, PerKey = this.PerKey, text = this.text });

        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class DBSelectCommandParameter : ILiquidizable
    {
        public string name;
        public string source;
        public string Parameter;
        public void ParseEle(XmlNode node)
        {

            this.name = node.Attr("name");
            this.source = node.Attr("source");
            this.Parameter = node.Attr("Parameter");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.source, new string[] { "Expr", "QueryString", "Session" }, new string[] { "Parameter" });

            }
        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Parameter = this.Parameter, name = this.name, source = this.source });

        }

    }



    public class Page : ILiquidizable
    {
        public string Title;
        public string id;
        public string link;
        public string ColumnCount;
        public string PerKey;
        public string type;
        public string DBCommand;
        public string title;
        public string name;
        public string queryString;
        public string ValueDbCommand;
        public string JSStart;
        public string SubmitBevParameter;
        public string ShowCond;
        public string SubmitBev;
        public string isDailog;
        public string Paging;
        public string PagingCountCommand;
        public string SerachPrevValuesInBack;
        public List<Note> Notes = new List<Note>();

        public List<PageParameter> PageParameters = new List<PageParameter>();
        public List<Button> Buttons = new List<Button>();
        public List<table> tables = new List<table>();
        public List<ValueParameter> ValueParameters = new List<ValueParameter>();
        public Dictionary<string, PageParameter> DicPageParameters = new Dictionary<string, PageParameter>();
        public List<CustomValidate> CustomValidates = new List<CustomValidate>();
        public List<BatchCommand> BatchCommands = new List<BatchCommand>();
        public Boolean NoneFormParameters = false;
        public Boolean MustSendFiles = false;
        public string HideDefaultButton = "";
        public int ValSize = 0;
        public int HerSize = 0;
        public List<CustomHtml> CustomHtmls = new List<CustomHtml>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.isDailog = node.Attr("isDailog");
            this.ShowCond = node.Attr("ShowCond");
            this.Title = node.Attr("Title");
            this.id = node.Attr("id");
            this.link = node.Attr("link");
            this.HideDefaultButton = node.Attr("HideDefaultButton");
            this.ColumnCount = node.Attr("ColumnCount");
            this.PerKey = node.Attr("PerKey");
            this.type = node.Attr("type");
            this.DBCommand = node.Attr("DBCommand");
            this.title = node.Attr("title");
            this.name = node.Attr("name");
            this.queryString = node.Attr("queryString");
            this.SubmitBevParameter = node.Attr("SubmitBevParameter");
            this.ValueDbCommand = node.Attr("ValueDbCommand");
            this.JSStart = node.Attr("JSStart");
            this.Paging = node.Attr("Paging");
            this.PagingCountCommand = node.Attr("PagingCountCommand");

            this.SerachPrevValuesInBack = node.Attr("SerachPrevValuesInBack");
            this.SubmitBev = node.Attr("SubmitBev");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "PageParameter")
                {
                    PageParameter Temp = new PageParameter();
                    Temp.ParseEle(node.ChildNodes[i]);
                    PageParameters.Add(Temp);
                    DicPageParameters.Add(Temp.name, Temp);
                    if ((Temp.source == "form") && (Temp.dontSendToDb != "Yes"))
                    {
                        NoneFormParameters = true;
                    }
                    if ((Temp.type == "FileInput"))
                    {

                        MustSendFiles = true;
                    }
                }
                if (node.ChildNodes[i].Name == "CustomValidate")
                {
                    CustomValidate Temp = new CustomValidate();
                    Temp.ParseEle(node.ChildNodes[i]);
                    CustomValidates.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Note")
                {
                    Note Temp = new Note();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Notes.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Button")
                {
                    Button Temp = new Button();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Buttons.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "table")
                {
                    table Temp = new table();
                    Temp.ParseEle(node.ChildNodes[i]);
                    tables.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "ValueParameter")
                {
                    ValueParameter Temp = new ValueParameter();
                    Temp.ParseEle(node.ChildNodes[i]);
                    ValueParameters.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "BatchCommand")
                {
                    BatchCommand Temp = new BatchCommand();


                    Temp.ParseEle(node.ChildNodes[i]);
                    BatchCommands.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CustomHtml")
                {
                    CustomHtml Temp = new CustomHtml();
                    Temp.InnerHtml = node.ChildNodes[i].InnerXml;
                    CustomHtmls.Add(Temp);

                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            string DBComParaUsage = "";/* PDASys.CreateParameterUsage(DBCommand);*/
            int h = 0, v = 0;
            switch (int.Parse(this.ColumnCount))
            {
                case 1:
                    h = 3;
                    v = 9;
                    break;
                case 2:
                    h = 2;
                    v = 4;
                    break;
                case 3:
                    h = 2;
                    v = 2;
                    break;
                case 4:
                    h = 1;
                    v = 2;
                    break;
                default:
                    h = (int)Math.Floor(((12f / float.Parse(ColumnCount)) * .75f));
                    v = (int)Math.Floor(((12f / float.Parse(ColumnCount)) * .25f));
                    break;
            }

            foreach (PageParameter pg in PageParameters)
            {
                if (pg.PutOnAllSpace != "Yes")
                {
                    pg.ValSize = v;
                    pg.HerSize = h;

                }
                else
                {

                    switch (int.Parse(this.ColumnCount))
                    {
                        case 1:
                            pg.ValSize = 9;
                            pg.HerSize = 3;
                            break;
                        case 2:
                            pg.ValSize = 10;
                            pg.HerSize = 2;
                            break;
                        case 3:
                            pg.ValSize = 10;
                            pg.HerSize = 2;
                            break;
                        case 4:
                            h = 1;
                            v = 2;
                            break;
                        default:

                            break;
                    }






                }

            }

        }



        public object ToLiquid()
        {
            foreach (BatchCommand b in BatchCommands)
            {
                foreach (Command C in b.Commands)
                {
                    foreach (Parameter P in C.Parameters)
                    {
                        if (P.sourceType == "PageParameter")
                        {
                            P.RealData = DicPageParameters[P.sourceTypeParameter].type;

                        }
                        if (P.sourceType == "Row")
                        {
                            foreach (table t in tables)
                            {
                                foreach (column c in t.columns)
                                {
                                    if (c.name == P.sourceTypeParameter)
                                    {
                                        P.RealData = c.type;
                                        break;
                                    }


                                }


                            }

                        }


                    }


                }

            }




            Util.InitCommanD(ref this.DBCommand, PageParameters.toInputParameter(), null);
            if (ValueDbCommand != "")
            {
                Util.InitCommanD(ref this.ValueDbCommand, ValueParameters.toInputParameter(), null);
            }
            return Hash.FromAnonymousObject(new { CustomHtmls = this.CustomHtmls, Paging = this.Paging, PagingCountCommand = this.PagingCountCommand, SerachPrevValuesInBack = this.SerachPrevValuesInBack, HideDefaultButton = this.HideDefaultButton, isDailog = this.isDailog, ShowCond = this.ShowCond, SubmitBev = this.SubmitBev, SubmitBevParameter = this.SubmitBevParameter, Notes = this.Notes, MustSendFiles = this.MustSendFiles.ToString(), BatchCommands = this.BatchCommands, NoneFormParameters = this.NoneFormParameters.ToString(), JSStart = this.JSStart, HerSize = this.HerSize, ValSize = this.ValSize, Title = this.Title, id = this.id, link = this.link, ColumnCount = this.ColumnCount, PerKey = this.PerKey, type = this.type, DBCommand = this.DBCommand, name = this.name, queryString = this.queryString, ValueDbCommand = this.ValueDbCommand, PageParameters = this.PageParameters, Buttons = this.Buttons, tables = this.tables, ValueParameters = this.ValueParameters, CustomValidates = this.CustomValidates });

        }

    }
    public class CustomValidate : ILiquidizable
    {
        public string Cond;
        public string Message;
        public void ParseEle(XmlNode node)
        {

            this.Cond = node.Attr("Cond");
            this.Message = node.Attr("Message");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Cond = this.Cond, Message = this.Message });

        }

    }

    public class CommandCustomValidate : ILiquidizable
    {
        public string Cond;
        public string Message;
        public string For;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Cond = node.Attr("Cond");
            this.Message = node.Attr("Message");
            this.For = node.Attr("For");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { For = this.For, Cond = this.Cond, Message = this.Message });

        }

    }






    public class PageParameter : ILiquidizable
    {
        public string title;
        public string name;
        public string type;
        public string source;
        public string sorurceParameter;
        public string startValueType;
        public string Parameter;
        public string SaveFileNameExpr;
        public string Disabled;
        public string DefaultValueSource;
        public string DefaultValueParameter;
        public List<ParameterCheck> ParameterChecks = new List<ParameterCheck>();
        public string DBSelect2Command;
        public string codeColumn;
        public string textColumn;
        public string TitleParameter;
        public string MaxFileSize;
        public string dontSendToDb = "";
        public string FilePathAtServer = "";
        public string LinkSyntax;
        public string Width;
        public string Height;
        public string DBSelect2CommandDriectValue;
        public string DBSelect2CommandDriectValueParameterName;
        public string AjaxActionReturnValuesName;
        public string AjaxActionReturnValuesParameterSyntax;
        public string AjaxActionReturnValuesValueColumn;
        public string AjaxActionReturnValuesTitleColumn;
        public string PlaceHolder;
        public List<DBSelectCommandParameter> DBSelectCommandParameters = new List<DBSelectCommandParameter>();
        public List<option> options = new List<option>();
        public List<FileAllow> FileAllows = new List<FileAllow>();
        public string ChangeBev;
        public string ChangeBevParameter;
        public string ShowCond;
        public List<Button> Buttons = new List<Button>();
        public string CustomControllType = "";
        public string PutOnAllSpace = "";
        public int HerSize;
        public int ValSize;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.SaveFileNameExpr = node.Attr("SaveFileNameExpr");
            this.title = node.Attr("title");
            this.name = node.Attr("name");
            this.ShowCond = node.Attr("ShowCond");
            this.type = node.Attr("type");
            this.CustomControllType = node.Attr("CustomControllType");
            this.AjaxActionReturnValuesName = node.Attr("AjaxActionReturnValuesName");
            this.AjaxActionReturnValuesParameterSyntax = node.Attr("AjaxActionReturnValuesParameterSyntax");
            this.AjaxActionReturnValuesTitleColumn = node.Attr("AjaxActionReturnValuesTitleColumn");
            this.AjaxActionReturnValuesValueColumn = node.Attr("AjaxActionReturnValuesValueColumn");
            this.PutOnAllSpace = node.Attr("PutOnAllSpace");
            this.source = node.Attr("source");
            this.Disabled = node.Attr("Disabled");
            this.DBSelect2CommandDriectValue = node.Attr("DBSelect2CommandDriectValue");

            this.DBSelect2CommandDriectValueParameterName = node.Attr("DBSelect2CommandDriectValueParameterName");
            this.ChangeBev = node.Attr("ChangeBev");
            this.ChangeBevParameter = node.Attr("ChangeBevParameter");
            this.dontSendToDb = node.Attr("dontSendToDb");
            this.sorurceParameter = node.Attr("sorurceParameter");
            this.startValueType = node.Attr("startValueType");
            this.Parameter = node.Attr("Parameter");
            DefaultValueSource = node.Attr("DefaultValueSource");
            PlaceHolder = node.Attr("PlaceHolder");
            DefaultValueParameter = node.Attr("DefaultValueParameter");
            this.DBSelect2Command = node.Attr("DBSelect2Command");
            this.codeColumn = node.Attr("codeColumn");
            this.textColumn = node.Attr("textColumn");
            MaxFileSize = node.Attr("MaxFileSize");
            this.TitleParameter = node.Attr("TitleParameter");
            FilePathAtServer = node.Attr("FilePathAtServer");
            LinkSyntax = node.Attr("LinkSyntax");
            Width = node.Attr("Width");
            Height = node.Attr("Height");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "ParameterCheck")
                {
                    ParameterCheck Temp = new ParameterCheck();
                    Temp.ParseEle(node.ChildNodes[i]);
                    ParameterChecks.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "DBSelectCommandParameter")
                {
                    DBSelectCommandParameter Temp = new DBSelectCommandParameter();
                    Temp.ParseEle(node.ChildNodes[i]);
                    DBSelectCommandParameters.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
                if (node.ChildNodes[i].Name == "option")
                {
                    option Temp = new option();
                    Temp.ParseEle(node.ChildNodes[i]);
                    options.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "FileAllow")
                {
                    FileAllow Temp = new FileAllow();
                    Temp.ParseEle(node.ChildNodes[i]);
                    FileAllows.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "Button")
                {
                    Button Temp = new Button();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Buttons.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {


                }
            }
            if (CustomControllType != "")
            {
                GenCustomControll(CustomControllType);
            }
            if (PDAL.App.Debug == "Yes")
            {
                if (source == "form")
                {
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "FileInput" }, new string[] { "MaxFileSize", "FilePathAtServer" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "ImageView", "DownloadLink" }, new string[] { "LinkSyntax" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "Select2Ajax", "Select2AjaxMulti" }, new string[] { "DBSelect2Command", "textColumn", "codeColumn" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "Select2AjaxMulti" }, new string[] { "AjaxActionReturnValuesValueColumn", "AjaxActionReturnValuesTitleColumn", "AjaxActionReturnValuesName", "AjaxActionReturnValuesParameterSyntax" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "DisplayExpr", "DisplayExprMoney" }, new string[] { "Parameter" });

                    ValidatorChecker.CheckItemCount(node, this.type, new string[] { "DropDown", "Select2", "Select2Multi" }, options.Count, "گزینه ها");
                    ValidatorChecker.CheckItemCount(node, this.type, new string[] { "Select2AjaxMulti", "Select2Ajax" }, DBSelectCommandParameters.Count, "گزینه ها");
                    ValidatorChecker.CheckItemCount(node, this.type, new string[] { "FileInput" }, FileAllows.Count, "نوع فایل ها");


                }
                else
                {
                    ValidatorChecker.CheckNotEmpty(node, this.source, new string[] { "QueryString" }, new string[] { "Parameter" });
                    ValidatorChecker.CheckNotEmpty(node, this.source, new string[] { "Session", "SpecValue", "Expr" }, new string[] { "sorurceParameter" });


                }
                ValidatorChecker.CheckNotEmpty(node, this.DefaultValueSource, new string[] { "Session", "SpecValue", "Expr", "form", "Static" }, new string[] { "DefaultValueParameter" });

            }
        }
        public object ToLiquid()
        {
            if (DBSelect2Command != "")
            {
                Util.InitCommanD(ref DBSelect2Command, DBSelectCommandParameters.toInputParameter(), null);
            }

            return Hash.FromAnonymousObject(new { ValSize = this.ValSize, HerSize = this.HerSize, PutOnAllSpace = this.PutOnAllSpace, CustomControllInitValue = this.CustomControllInitValue, CustomControllhtml = this.CustomControllhtml, CustomControllval = this.CustomControllval, CustomControllgetValue = this.CustomControllgetValue, CustomControllsetValue = this.CustomControllsetValue, AjaxActionReturnValuesName = this.AjaxActionReturnValuesName, AjaxActionReturnValuesParameterSyntax = this.AjaxActionReturnValuesParameterSyntax, AjaxActionReturnValuesTitleColumn = this.AjaxActionReturnValuesTitleColumn, AjaxActionReturnValuesValueColumn = this.AjaxActionReturnValuesValueColumn, SaveFileNameExpr = this.SaveFileNameExpr, DBSelect2CommandDriectValueParameterName = this.DBSelect2CommandDriectValueParameterName, DBSelect2CommandDriectValue = this.DBSelect2CommandDriectValue, ChangeBevParameter = this.ChangeBevParameter, Buttons = this.Buttons, ShowCond = this.ShowCond, ChangeBev = this.ChangeBev, PlaceHolder = this.PlaceHolder, Width = this.Width, Height = this.Height, LinkSyntax = this.LinkSyntax, FileAllows = this.FileAllows, FilePathAtServer = this.FilePathAtServer, MaxFileSize = this.MaxFileSize, Disabled = this.Disabled, dontSendToDb = this.dontSendToDb, options = this.options, TitleParameter = this.TitleParameter, title = this.title, name = this.name, type = this.type, source = this.source, DefaultValueSource = this.DefaultValueSource, DefaultValueParameter = this.DefaultValueParameter, sorurceParameter = this.sorurceParameter, startValueType = this.startValueType, Parameter = this.Parameter, DBSelect2Command = this.DBSelect2Command, codeColumn = this.codeColumn, textColumn = this.textColumn, ParameterChecks = this.ParameterChecks, DBSelectCommandParameters = this.DBSelectCommandParameters });

        }
        public void GenCustomControll(string Type)
        {


        }


        public string CustomControllhtml, CustomControllval, CustomControllgetValue, CustomControllsetValue, CustomControllInitValue;

    }


    public class FileAllow : ILiquidizable
    {
        public string ext;

        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.ext = node.Attr("ext");


        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { ext = this.ext });
        }

    }



    public class option : ILiquidizable
    {
        public string value;
        public string text;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.value = node.Attr("value");
            this.text = node.Attr("text");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { value = this.value, text = this.text });

        }

    }
    public class CustomHtml : ILiquidizable
    {
        public string InnerHtml = "";



        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { InnerHtml = this.InnerHtml });

        }
    }

    public class DontBuild : ILiquidizable
    {
        public string FileName;

        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.FileName = node.Attr("FileName");

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { FileName = this.FileName });

        }

    }


    public class DisplayExpr : ILiquidizable
    {
        public string title;
        public string Expr;
        public void ParseEle(XmlNode node)
        {

            this.title = node.Attr("title");
            this.Expr = node.Attr("Expr");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { title = this.title, Expr = this.Expr });

        }

    }
    public class NewRecordColumnValue : ILiquidizable
    {
        public string ColumnName;
        public string value;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.ColumnName = node.Attr("ColumnName");
            this.value = node.Attr("value");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { value = this.value, ColumnName = this.ColumnName });

        }

    }
    public class Note : ILiquidizable
    {
        public string Text;
        public string FileName;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Text = node.Attr("Text");
            this.FileName = node.Attr("FileName");

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { FileName = this.FileName, Text = this.Text });

        }

    }
    public class Button : ILiquidizable
    {
        public string title;
        public string action;
        public string actionParameter;
        public string actionParameter2;
        public string actionParameter3;
        public string Icon;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.title = node.Attr("title");
            this.action = node.Attr("action");
            this.actionParameter = node.Attr("actionParameter");
            this.actionParameter2 = node.Attr("actionParameter2");
            this.actionParameter3 = node.Attr("actionParameter3");
            this.Icon = node.Attr("Icon");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.action, new string[] { "goToPage", "dynLink", "JavaScript", "BatchCommand", "ShowDailogAndPutValue", "ShowDailogAndSelectValue", "ShowDailogAndSelectValueSelect2" }, new string[] { "actionParameter" });


            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.action, new string[] { "ShowDailogAndPutValue", "ShowDailogAndSelectValue", "ShowDailogAndSelectValueSelect2" }, new string[] { "actionParameter", "actionParameter2", "actionParameter3" });


            }
        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Icon = this.Icon, title = this.title, action = this.action, actionParameter2 = this.actionParameter2, actionParameter3 = this.actionParameter3, actionParameter = this.actionParameter });

        }

    }
    public class column : ILiquidizable
    {
        public string title;
        public string name;
        public string type;
        public string Caption;
        public string linkSyntax;
        public string AskMessage;
        public string AjaxAction;
        public string ParameterSyntax;
        public string DBSelect2Command;
        public string codeColumn;
        public string textColumn;
        public string Width;
        public string Height;
        public string ShowCond;
        public string TitleParameter;
        public string StaticJavaScriptAfterChange;
        public string AjaxActionChange = "";
        public string Icon = "";

        public List<CustomHtml> CustomHtmls = new List<CustomHtml>();
        public List<DBSelectCommandParameter> DBSelectCommandParameters = new List<DBSelectCommandParameter>();
        public List<option> options = new List<option>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.title = node.Attr("title");
            this.name = node.Attr("name");
            this.type = node.Attr("type");
            this.Caption = node.Attr("Caption");
            ShowCond = node.Attr("ShowCond");
            StaticJavaScriptAfterChange = node.Attr("StaticJavaScriptAfterChange");
            this.linkSyntax = node.Attr("linkSyntax");
            this.AskMessage = node.Attr("AskMessage");
            this.AjaxAction = node.Attr("AjaxAction");
            this.DBSelect2Command = node.Attr("DBSelect2Command");
            this.codeColumn = node.Attr("codeColumn");
            this.textColumn = node.Attr("textColumn");
            this.Width = node.Attr("Width");
            this.Height = node.Attr("Height");
            this.TitleParameter = node.Attr("TitleParameter");
            this.Icon = node.Attr("Icon");
            this.ParameterSyntax = node.Attr("ParameterSyntax");
            AjaxActionChange = node.Attr("AjaxActionChange");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "option")
                {
                    option Temp = new option();
                    Temp.ParseEle(node.ChildNodes[i]);
                    options.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "DBSelectCommandParameter")
                {
                    DBSelectCommandParameter Temp = new DBSelectCommandParameter();
                    Temp.ParseEle(node.ChildNodes[i]);
                    DBSelectCommandParameters.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CustomHtml")
                {
                    CustomHtml Temp = new CustomHtml();
                    Temp.InnerHtml = node.ChildNodes[i].InnerXml;
                    CustomHtmls.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }

                if (PDAL.App.Debug == "Yes")
                {
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "Expr", "RunningTotalValues" }, new string[] { "ParameterSyntax" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "Image", "DownloadFile" }, new string[] { "linkSyntax" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "InputSelect2Ajax" }, new string[] { "DBSelect2Command", "codeColumn", "textColumn" });
                    ValidatorChecker.CheckNotEmpty(node, this.type, new string[] { "AjaxAction" }, new string[] { "AjaxAction" });
                    ValidatorChecker.CheckItemCount(node, this.type, new string[] { "InputSelect2", "InputCombo", "RangeValues" }, options.Count, "گزینه ها");
                    ValidatorChecker.CheckItemCount(node, this.type, new string[] { "InputSelect2Ajax" }, DBSelectCommandParameters.Count, "گزینه ها");

                }
            }

        }
        public object ToLiquid()
        {
            if (this.DBSelect2Command != "")
            {
              List<inputParameter> ix=new List<inputParameter>();
                foreach( DBSelectCommandParameter DB in DBSelectCommandParameters)
                {
                    ix.Add(new inputParameter(DB.name,"0"));

                }



                Util.InitCommanD(ref DBSelect2Command, ix, null);
            }
            return Hash.FromAnonymousObject(new { Icon = this.Icon, CustomHtmls = this.CustomHtmls, ShowCond = this.ShowCond, Width = this.Width, Height = this.Height, AjaxActionChange = this.AjaxActionChange, StaticJavaScriptAfterChange = this.StaticJavaScriptAfterChange, DBSelectCommandParameters = this.DBSelectCommandParameters, DBSelect2Command = this.DBSelect2Command, codeColumn = this.codeColumn, textColumn = this.textColumn, TitleParameter = this.TitleParameter, options = this.options, title = this.title, name = this.name, type = this.type, Caption = this.Caption, linkSyntax = this.linkSyntax, AskMessage = this.AskMessage, AjaxAction = this.AjaxAction, ParameterSyntax = this.ParameterSyntax });

        }

    }
    public class ParameterCheck : ILiquidizable
    {
        public string Cond;
        public string Value;
        public string Value2;
        public string When;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.Cond = node.Attr("Cond");
            this.Value = node.Attr("Value");
            this.Value2 = node.Attr("Value2");
            this.When = node.Attr("When");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Cond = this.Cond, Value = this.Value, Value2 = this.Value2, When = this.When });

        }

    }
    public class ValueParameter : ILiquidizable
    {
        public string name;
        public string value;
        public string source;
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            this.name = node.Attr("name");
            this.value = node.Attr("value");
            this.source = node.Attr("source");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { name = this.name, value = this.value, source = this.source });

        }

    }
    public class action : ILiquidizable
    {
        public string name;
        public string DBCommand;
        public string PerKey;

        public List<actionParameter> actionParameters = new List<actionParameter>();
        public void ParseEle(XmlNode node)
        {

            this.name = node.Attr("name");
            this.DBCommand = node.Attr("DBCommand");
            this.PerKey = node.Attr("PerKey");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "actionParameter")
                {
                    actionParameter Temp = new actionParameter();
                    Temp.ParseEle(node.ChildNodes[i]);
                    actionParameters.Add(Temp);
                }

            }

        }
        public object ToLiquid()
        {
            Util.InitCommanD(ref DBCommand, actionParameters.toInputParameter(), null);
            return Hash.FromAnonymousObject(new { name = this.name, DBCommand = this.DBCommand, PerKey = this.PerKey, actionParameters = this.actionParameters });

        }

    }
    public class actionParameter : ILiquidizable
    {
        public string name;
        public string type;
        public string datatype;
        public string Parameter;
        public void ParseEle(XmlNode node)
        {

            this.name = node.Attr("name");
            this.type = node.Attr("type");
            this.datatype = node.Attr("datatype");
            this.Parameter = node.Attr("Parameter");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { name = this.name, type = this.type, datatype = this.datatype, Parameter = this.Parameter });

        }

    }
    public class document : ILiquidizable
    {
        public List<App> Applications = new List<App>();
        public void ParseEle(XmlNode node)
        {

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Application")
                {
                    App Temp = new App();
                    Temp.ParseEle(node.ChildNodes[i]);
                    Applications.Add(Temp);
                }

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Applications = this.Applications });

        }

    }
    public class Pages : ILiquidizable
    {
        public List<Page> _Pages = new List<Page>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "Page")
                {
                    Page Temp = new Page();
                    Temp.ParseEle(node.ChildNodes[i]);
                    _Pages.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { Pages = this._Pages });

        }

    }
    public class Actions : ILiquidizable
    {
        public List<action> actions = new List<action>();
        public void ParseEle(XmlNode node)
        {

            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "action")
                {
                    action Temp = new action();
                    Temp.ParseEle(node.ChildNodes[i]);
                    actions.Add(Temp);
                    if (PDAL.actionAccess.ContainsKey(Temp.name) == false)
                    {
                        PDAL.actionAccess.Add(Temp.name, Temp);
                    }
                }

            }

        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { actions = this.actions });

        }

    }
    public class table : ILiquidizable
    {

        public string Sortable = "";
        public string BasicFilter = "";
        public string HideActionColumnn = "";
        public string RowNumberColumn = "";
        public string Selectable = "";
        public string Insertable = "";
        public string AutoSelectCond = "";
        public string DailogSelectable = "";
        public string PoivtTable = "";
        public string PoivtRowColumn = "";
        public string PoivtColumnName = "";
        public string PoivtValueName = "";
        public List<column> columns = new List<column>();
        public List<DisplayExpr> DisplayExprs = new List<DisplayExpr>();
        public List<NewRecordColumnValue> NewRecordColumnValues = new List<NewRecordColumnValue>();
        public void ParseEle(XmlNode node)
        {
            string ExtendedFrom = node.Attr("ExtendedFrom");
            if (ExtendedFrom != "")
            {
                XmlNode X = PDAL.RootElement.SelectSingleNode(ExtendedFrom);
                PDAL.NodeMerge(X, node);
            }
            Sortable = node.Attr("Sortable");
            BasicFilter = node.Attr("BasicFilter");
            HideActionColumnn = node.Attr("HideActionColumnn");
            RowNumberColumn = node.Attr("RowNumberColumn");
            Selectable = node.Attr("Selectable");
            Insertable = node.Attr("Insertable");
            AutoSelectCond = node.Attr("AutoSelectCond");
            PoivtColumnName = node.Attr("PoivtColumnName");
            PoivtRowColumn = node.Attr("PoivtRowColumn");
            PoivtTable = node.Attr("PoivtTable");
            PoivtValueName = node.Attr("PoivtValueName");

            DailogSelectable = node.Attr("DailogSelectable");
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                if (node.ChildNodes[i].Name == "column")
                {
                    column Temp = new column();
                    Temp.ParseEle(node.ChildNodes[i]);
                    columns.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "DisplayExpr")
                {
                    DisplayExpr Temp = new DisplayExpr();
                    Temp.ParseEle(node.ChildNodes[i]);
                    DisplayExprs.Add(Temp);
                }

                if (node.ChildNodes[i].Name == "NewRecordColumnValue")
                {
                    NewRecordColumnValue Temp = new NewRecordColumnValue();
                    Temp.ParseEle(node.ChildNodes[i]);
                    NewRecordColumnValues.Add(Temp);
                }
                if (node.ChildNodes[i].Name == "CopyElements")
                {
                    XmlNode X = PDAL.RootElement.SelectSingleNode(node.ChildNodes[i].Attr("XPath"));
                    String Attr = node.ChildNodes[i].Attr("Attr");
                    for (int z = X.ChildNodes.Count - 1; z >= 0; z--)
                    {

                        XmlNode zQ = X.ChildNodes[z].Clone();
                        if (((Attr == "*") || (zQ.Name == Attr)) && (zQ.Name != "CopyElements"))
                        {
                            node.InsertAfter(zQ, node.ChildNodes[i]);
                        }
                    }
                }
            }
            if (PDAL.App.Debug == "Yes")
            {
                ValidatorChecker.CheckNotEmpty(node, this.PoivtTable, new string[] { "Yes" }, new string[] { "PoivtRowColumn", "PoivtValueName", "PoivtColumnName" });
                ValidatorChecker.CheckItemCount(node, columns.Count, "ستون ها");

            }
        }
        public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new { PoivtTable = this.PoivtTable, PoivtRowColumn = this.PoivtRowColumn, PoivtColumnName = this.PoivtColumnName, PoivtValueName = this.PoivtValueName, DailogSelectable = this.DailogSelectable, DisplayExprs = this.DisplayExprs, AutoSelectCond = this.AutoSelectCond, Insertable = this.Insertable, NewRecordColumnValues = this.NewRecordColumnValues, Selectable = this.Selectable, RowNumberColumn = this.RowNumberColumn, HideActionColumnn = this.HideActionColumnn, BasicFilter = this.BasicFilter, Sortable = this.Sortable, columns = this.columns });

        }

    }
}