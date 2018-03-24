using EnvDTE;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZActions
{
    class EZActionsUtilities
    {
        public static List<Type> GetAllTypes(IServiceProvider provider)
        {
            var trs = GetTypeDiscoveryService(provider);
            var types = trs.GetTypes(typeof(object), true);
            var result = new List<Type>();
            foreach (Type type in types)
            {
                if (type.IsPublic)
                {
                    if (!result.Contains(type))
                        result.Add(type);
                }
            }
            return result;
        }

        private static ITypeDiscoveryService GetTypeDiscoveryService(IServiceProvider provider)
        {
            var dte = GetService<EnvDTE.DTE>(provider);
            var typeService = GetService<DynamicTypeService>(provider);
            var solution = GetService<IVsSolution>(provider);
            IVsHierarchy hier;
            var projects = dte.ActiveSolutionProjects as Array;
            var currentProject = projects.GetValue(0) as Project;
            solution.GetProjectOfUniqueName(currentProject.UniqueName, out hier);
            return typeService.GetTypeDiscoveryService(hier);
        }

        private static T GetService<T>(IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}
