using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DllReader2.AssemblyData
{
    public class AssemblyDllData
    {
        #region Fields
        private Assembly assembly;

        private AssemblyTypeData[] typesData;
        private string[] typesNames;
        #endregion

        #region Funcs
        public AssemblyDllData(Assembly assembly)
        {
            this.assembly = assembly;

            Type[] types = assembly.GetTypes();
            typesData = new AssemblyTypeData[types.Length];
            typesNames = new string[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                typesData[i] = new AssemblyTypeData(types[i], this);
                typesNames[i] = typesData[i].TypeName;
            }
        }

        public override string ToString()
        {
            return AssemblyName;
        }
        #endregion

        #region Props
        public Assembly Assembly { get { return assembly; } }
        public string AssemblyName { get { return assembly.ManifestModule.Name; } }

        public AssemblyTypeData[] Types { get { return typesData; } }
        public string[] TypesNames { get { return typesNames; } }
        #endregion
    }
}
