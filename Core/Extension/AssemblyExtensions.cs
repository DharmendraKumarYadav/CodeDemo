using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extension
{
    public static class AssemblyExtensions
    {
        private static readonly object _asmDepsLock = new object();
        private static readonly Dictionary<string, List<string>> _asmDeps = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);



        /// <summary>
        /// Returns the loadable types from the assembly
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="exportedTypesOnly">Whether to return only exported/public types</param>
        public static TypeInfo[] GetLoadableTypes(this Assembly assembly, bool exportedTypesOnly = false)
        {
            List<Type> typeList = new List<Type>();
            try
            {
                typeList = assembly.GetTypes()
                    .Where(t => !exportedTypesOnly || t.IsVisible)
                    .ToList();
            }
            catch (ReflectionTypeLoadException )
            {
                typeList = new List<Type>();
            }
            catch
            {
                typeList = new List<Type>();
            }
            return typeList.Select(t => t.GetTypeInfo()).ToArray();
        }


    }
}
