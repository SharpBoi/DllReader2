using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DllReader2.AssemblyData
{
    public enum TypeKeyWords { Class, Struct, Interface, Enum }

    public class AssemblyTypeData
    {
        #region Fields
        private Type type;
        private AssemblyDllData parentDll;
        private TypeKeyWords typeKeyWord;

        private AssemblyMethodData[] allMethods;
        private AssemblyMethodData[] publicMethods;
        private AssemblyMethodData[] protectedMethods;

        private AssemblyMethodData[] declaredMethods;
        private AssemblyMethodData[] declaredPublic;
        private AssemblyMethodData[] declaredProtected;
        #endregion

        #region Funcs
        public AssemblyTypeData(Type type, AssemblyDllData parentDll)
        {
            this.type = type;
            this.parentDll = parentDll;

            // Enum, Object(class), ValueType(struct), null(interface)
            if (type.BaseType == null)
                typeKeyWord = TypeKeyWords.Interface;
            else if (type.BaseType.Name == "Enum")
                typeKeyWord = TypeKeyWords.Enum;
            else if (type.BaseType.Name == "Object")
                typeKeyWord = TypeKeyWords.Class;
            else if (type.BaseType.Name == "ValueType")
                typeKeyWord = TypeKeyWords.Struct;

            parseMethods(type.GetRuntimeMethods().Cast<MethodInfo>() as MethodInfo[]);
        }

        private void parseMethods(MethodInfo[] importedMethods)
        {
            List<AssemblyMethodData> declaredPubs = new List<AssemblyMethodData>();
            List<AssemblyMethodData> declaredProtecs = new List<AssemblyMethodData>();
            List<AssemblyMethodData> declaredMethods = new List<AssemblyMethodData>();
            List<AssemblyMethodData> pubMethods = new List<AssemblyMethodData>();
            List<AssemblyMethodData> protecMethods = new List<AssemblyMethodData>();

            allMethods = new AssemblyMethodData[importedMethods.Length];
            for (int i = 0; i < allMethods.Length; i++)
            {
                MethodInfo method = importedMethods[i];

                allMethods[i] = new AssemblyMethodData(method, this);

                if (method.DeclaringType == type)
                    declaredMethods.Add(allMethods[i]);

                if (allMethods[i].AccessModifier.HasFlag(MethodAttributes.Public))
                {
                    pubMethods.Add(allMethods[i]);
                    if (allMethods[i].MethodInfo.DeclaringType == type)
                        declaredPubs.Add(allMethods[i]);
                }
                else if (allMethods[i].AccessModifier.HasFlag(MethodAttributes.Family))
                {
                    protecMethods.Add(allMethods[i]);
                    if (allMethods[i].MethodInfo.DeclaringType == type)
                        declaredProtecs.Add(allMethods[i]);
                }
            }

            publicMethods = pubMethods.ToArray();
            protectedMethods = protecMethods.ToArray();

            this.declaredMethods = declaredMethods.ToArray();
            this.declaredPublic = declaredPubs.ToArray();
            this.declaredProtected = declaredProtecs.ToArray();
        }

        public override string ToString()
        {
            if (TypeDescriptionMode == DescriptionMode.Full)
                return TypeKeyWordName + " " + TypeName;
            else if (TypeDescriptionMode == DescriptionMode.Compact)
                return TypeName;

            return "unknown";
        }
        #endregion

        #region Props
        public DescriptionMode TypeDescriptionMode { get; set; }

        public Type Type { get { return type; } }
        public string TypeName
        {
            get
            {
                return type.Name;
            }
        }
        public TypeKeyWords TypeKeyWord { get { return typeKeyWord; } }
        public string TypeKeyWordName { get { return typeKeyWord.ToString().ToLower(); } }

        public AssemblyMethodData[] AllMethods { get { return allMethods; } }
        public AssemblyMethodData[] AllPublicMethods { get { return publicMethods.ToArray(); } }
        public AssemblyMethodData[] AllProtectedMethods { get { return protectedMethods.ToArray(); } }

        public AssemblyMethodData[] DeclaredMethods { get { return declaredMethods; } }
        public AssemblyMethodData[] DeclaredPublicMethods { get { return declaredPublic; } }
        public AssemblyMethodData[] DeclaredProtectedMethods { get { return declaredProtected; } }
        #endregion
    }
}
