using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DllReader2.AssemblyData
{
    public class AssemblyMethodData
    {
        #region Fields
        private MethodInfo info;
        private AssemblyTypeData parentType;

        private ParameterInfo[] args;
        private Type[] argsTypes;
        private string[] argsNames;

        private string argsFullString = "";
        private string methodFullString = "";
        private string accessModifierName = "";
        #endregion

        #region Funcs
        public AssemblyMethodData(MethodInfo info, AssemblyTypeData parentType)
        {
            this.info = info;
            this.parentType = parentType;

            parseMethodAccessModifier();
            parseMethodArgs();

            methodFullString = AccessModifierName + " " + ReturnType.Name + " " + MethodName + " (" + argsFullString + ")";
        }

        private void parseMethodAccessModifier()
        {
            // parse name of access modifier of this method
            if (info.Attributes.HasFlag(MethodAttributes.Family | MethodAttributes.Assembly))
                accessModifierName = "protected internal";
            else if (info.Attributes.HasFlag(MethodAttributes.Private | MethodAttributes.Family))
                accessModifierName = "private protected";
            else if (info.Attributes.HasFlag(MethodAttributes.Public))
                accessModifierName = "public";
            else if (info.Attributes.HasFlag(MethodAttributes.Family))
                accessModifierName = "protected";
            else if (info.Attributes.HasFlag(MethodAttributes.Private))
                accessModifierName = "private";
            else if (info.Attributes.HasFlag(MethodAttributes.Assembly))
                accessModifierName = "internal";
        }
        private void parseMethodArgs()
        {
            args = info.GetParameters();
            argsTypes = new Type[args.Length];
            argsNames = new string[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                argsTypes[i] = args[i].ParameterType;
                argsNames[i] = args[i].Name;

                argsFullString += argsTypes[i].Name + " " + argsNames[i];
                if (i != args.Length - 1)
                    argsFullString += ", ";
            }
        }

        public override string ToString()
        {
            if (MethodDescriptionMode == DescriptionMode.Full)
                return methodFullString;
            else if (MethodDescriptionMode == DescriptionMode.Compact)
                return MethodName;

            return "unknown";
        }
        #endregion

        #region Props
        public DescriptionMode MethodDescriptionMode { get; set; }

        public MethodInfo MethodInfo { get { return info; } }

        public MethodAttributes AccessModifier
        {
            get
            {
                return info.Attributes;
            }
        }
        public string AccessModifierName
        {
            get
            {
                return accessModifierName;
            }
        }
        public Type ReturnType
        {
            get
            {
                return info.ReturnType;
            }
        }
        public string MethodName
        {
            get
            {
                return info.Name;
            }
        }

        public ParameterInfo[] Arguments
        {
            get { return args; }
        }
        #endregion
    }
}
