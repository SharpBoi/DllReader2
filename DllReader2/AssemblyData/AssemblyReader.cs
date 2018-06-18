using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DllReader2.AssemblyData
{
    public class AssemblyReader
    {
        public List<AssemblyDllData> ReadDllFolder(string assembliesDirectory, List<Exception> excs = null)
        {
            List<AssemblyDllData> dlls = new List<AssemblyDllData>();

            string[] filesNames = Directory.GetFiles(assembliesDirectory);
            FileInfo[] filesInfos = new FileInfo[filesNames.Length];
            for (int i = 0; i < filesInfos.Length; i++)
            {
                FileInfo info = new FileInfo(filesNames[i]);

                if (info.Extension.ToLower() == ".dll")
                {
                    try
                    {
                        dlls.Add(new AssemblyDllData(Assembly.LoadFile(info.FullName)));
                    }
                    catch(Exception ex)
                    {
                        if (excs != null) excs.Add(ex);
                    }
                }
            }

            return dlls;
        }

        public AssemblyDllData ReadDll(string path)
        {
            if (new FileInfo(path).Extension.ToLower() != ".dll")
                throw new FileFormatException();

            return new AssemblyDllData(Assembly.LoadFile(path));
        }

        public List<AssemblyDllData> ReadDlls(string[] paths, List<Exception> excs = null)
        {
            List<AssemblyDllData> ret = new List<AssemblyDllData>();

            for (int i = 0; i < paths.Length; i++)
                try
                {
                    ret.Add(ReadDll(paths[i]));
                }
                catch(Exception ex)
                {
                    if (excs != null) excs.Add(ex);
                }

            return ret;
        }
    }
}
