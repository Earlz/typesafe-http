//<#+
/*
Copyright (c) 2013 Jordan "Earlz/hckr83" Earls  <http://earlz.net>
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright
   notice, this list of conditions and the following disclaimer in the
   documentation and/or other materials provided with the distribution.
   
THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL
THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#if NOT_IN_T4
//Apparently T4 places classes into another class, making namespaces impossible
namespace Earlz.FluentHttp.Internal
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.IO;
#endif

    public class FluentGenerator
    {
        public FluentGenerator(string name)
        {

        }
        public void AddMethod(string name, string connectsTo, List<string> removes = null)
        {

        }

    }








//shove this all into one file so we don't force implementers to hand combine this or copy over more than 2 files
public class ClassGenerator : CodeElement
{
    virtual public List<Property> Properties
    {
        get;
        private set;
    }
    virtual public List<Method> Methods
    {
        get;
        private set;
    }
    virtual public List<Field> Fields
    {
        get;
        private set;
    }
    virtual public string Namespace
    {
        get;
        set;
    }
    virtual public string OtherCode
    {
        get;
        set;
    }
    public virtual string BaseClass
    {
        get;
        set;
    }
    public virtual List<ClassGenerator> NestedClasses
    {
        get;
        private set;
    }
    public ClassGenerator()
    {
        Properties = new List<Property>();
        Methods = new List<Method>();
        Fields = new List<Field>();
        NestedClasses = new List<ClassGenerator>();
        Accessibility = "";
    }
    public override string ToString()
    {
        return ToString(true);
    }
    public virtual string ToString(bool includenamespace)
    {
        StringBuilder sb = new StringBuilder();
        if (includenamespace)
        {
            sb.Append("namespace " + Namespace);
            sb.AppendLine("{");
        }
        sb.AppendLine(PrefixDocs);
        sb.AppendLine(GetTab(1) + Accessibility + " class " + Name + ": " + BaseClass);
        sb.AppendLine(GetTab(1) + "{");
        foreach (var p in Properties)
        {
            sb.AppendLine(p.ToString());
        }
        foreach (var m in Methods)
        {
            sb.AppendLine(m.ToString());
        }
        foreach (var f in Fields)
        {
            sb.AppendLine(f.ToString());
        }
        foreach (var c in NestedClasses)
        {
            sb.AppendLine(c.ToString(false));
        }
        sb.AppendLine(OtherCode);
        sb.AppendLine(GetTab(1) + "}");
        if (includenamespace)
        {
            sb.AppendLine("}");
        }
        return sb.ToString();
    }
}
public class InterfaceGenerator : ClassGenerator
{
    public override string ToString()
    {
        return ToString(true);
    }

    public override string ToString(bool includenamespace)
    {
        StringBuilder sb = new StringBuilder();
        if (includenamespace)
        {
            sb.Append("namespace " + Namespace);
            sb.AppendLine("{");
        }
        sb.AppendLine(PrefixDocs);
        sb.AppendLine(GetTab(1) + Accessibility + " interface " + Name + ": " + BaseClass);
        sb.AppendLine(GetTab(1) + "{");
        foreach (var p in Properties)
        {
            sb.AppendLine(p.ToString());
        }
        foreach (var m in Methods)
        {
            sb.AppendLine(m.ToString());
        }
        foreach (var f in Fields)
        {
            throw new NotSupportedException("Fields are not supported on interfaces");
        }
        foreach (var c in NestedClasses)
        {
            throw new NotSupportedException("Nested classes are not supported on interfaces");
        }
        sb.AppendLine(OtherCode);
        sb.AppendLine(GetTab(1) + "}");
        if (includenamespace)
        {
            sb.AppendLine("}");
        }
        return sb.ToString();
    }
}
abstract public class CodeElement
{
    public const string Tab = "    ";
    public string Name
    {
        get;
        set;
    }
    public string Accessibility
    {
        get;
        set;
    }
    string prefixdocs;
    virtual public string PrefixDocs
    {
        get
        {
            return prefixdocs;
        }
        set
        {
            prefixdocs = GetTab(2) + "///<summary>\n" + GetTab(2) + "///" + value + "\n" + GetTab(2) + "///</summary>";
        }
    }
    public override string ToString()
    {
        throw new NotImplementedException();
    }
    public static string GetTab(int nest)
    {
        string tmp = "";
        for (int i = 0; i < nest; i++)
        {
            tmp += Tab;
        }
        return tmp;
    }
    protected CodeElement()
    {
        Accessibility = "";
        PrefixDocs = "";
    }
}
public class Property : CodeElement
{
    public string Type
    {
        get;
        set;
    }
    public string GetMethod
    {
        get;
        set;
    }
    public string SetMethod
    {
        get;
        set;
    }
    public override string ToString()
    {
        string tmp = GetTab(2) + PrefixDocs + "\n";
        tmp += GetTab(2) + CodeElement.Tab + Accessibility + " " + Type + " " + Name + "{\n";
        if (GetMethod != null)
        {
            tmp += GetTab(2) + GetMethod + "\n";
        }
        if (SetMethod != null)
        {
            tmp += GetTab(2) + SetMethod + "\n";
        }
        tmp += GetTab(2) + "}\n";
        return tmp;
    }
    public Property()
    {
        GetMethod = "get;";
        SetMethod = "set;";
    }
    public Property CloneForInterface()
    {
        //Yes this is a hack. No there isn't a better foreseeable way around it
        var p = new Property();
        p.Accessibility = "";
        if (GetMethod.StartsWith("get"))
        {
            p.GetMethod = "get;";
        }
        else
        {
            p.GetMethod = "";
        }
        if (SetMethod.StartsWith("set"))
        {
            p.SetMethod = "set;";
        }
        else
        {
            p.SetMethod = "";
        }
        p.Name = Name;
        p.SetMethod = SetMethod;
        p.Type = Type;
        return p;
    }
}
public class Field : CodeElement
{
    public string Type
    {
        get;
        set;
    }
    public string InitialValue
    {
        get;
        set;
    }
    public override string ToString()
    {
        string tmp = GetTab(2) + PrefixDocs + "\n";
        tmp += GetTab(2) + Accessibility + " " + Type + " " + Name;
        if (InitialValue != null)
        {
            tmp += "=" + InitialValue + ";";
        }
        else
        {
            tmp += ";";
        }
        return tmp;
    }
}
public class Method : CodeElement
{
    public string ReturnType
    {
        get;
        set;
    }
    public List<MethodParam> Params
    {
        get;
        set;
    }
    public string Body
    {
        get;
        set;
    }
    public Method()
    {
        Params = new List<MethodParam>();
        Body = "";
        ReturnType = "void";
    }
    public override string ToString()
    {
        string tmp = GetTab(2) + PrefixDocs + "\n";
        tmp = GetTab(2) + Accessibility + " " + ReturnType + " " + Name + "(";
        for (int i = 0; i < Params.Count; i++)
        {
            tmp += Params[i].ToString();
            if (i == Params.Count - 1)
            {
                tmp += ")";
            }
            else
            {
                tmp += ", ";
            }
        }
        if (Params.Count == 0)
        {
            tmp += ")";
        }
        tmp += "\n" + GetTab(2) + "{\n";
        tmp += Body;
        tmp += "\n" + GetTab(2) + "}";
        return tmp;
    }
}
public class MethodParam
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Default { get; set; }
    public override string ToString()
    {
        string s = Type + " " + Name;
        if (Default != null)
        {
            s += "=" + Default;
        }
        return s;
    }
}

#if NOT_IN_T4
}
#endif
//#>