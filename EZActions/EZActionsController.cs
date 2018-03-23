using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EZActions
{
    public class EZActionsController
    {
        private IServiceProvider provider;

        public EZActionsController(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public List<Type> GetAllTypes()
        {
            var trs = GetTypeDiscoveryService();
            var types = trs.GetTypes(typeof(object), true /*excludeGlobalTypes*/);
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

        private ITypeDiscoveryService GetTypeDiscoveryService()
        {
            var dte = GetService<EnvDTE.DTE>();
            var typeService = GetService<DynamicTypeService>();
            var solution = GetService<IVsSolution>();
            IVsHierarchy hier;
            var projects = dte.ActiveSolutionProjects as Array;
            var currentProject = projects.GetValue(0) as Project;
            solution.GetProjectOfUniqueName(currentProject.UniqueName, out hier);
            return typeService.GetTypeDiscoveryService(hier);
        }

        private T GetService<T>()
        {
            return (T)provider.GetService(typeof(T));
        }

        public void Generate()
        {
            try
            {
                if(provider == null)
                {
                    ShowMessage("It doesn't seem like you have any MVC Project open. Please open an MVC Project, then try again!", OLEMSGICON.OLEMSGICON_WARNING);
                    return;
                }

                DTE dte = (DTE)provider.GetService(typeof(DTE));

                CodeElements ce = dte.Solution.Projects.Item(1).CodeModel.CodeElements;


                if(dte == null || dte.Solution == null || dte.Solution.IsOpen == false)
                {
                    ShowMessage("It doesn't seem like you have any MVC Project open. Please open an MVC Project, then try again!", OLEMSGICON.OLEMSGICON_WARNING);
                    return;
                }

                string solutionDir = System.IO.Path.GetDirectoryName(dte.Solution.FullName);
                StringBuilder ezActionsScript = new StringBuilder();

                List<Type> types = GetAllTypes().Where(p => p.BaseType != null && p.BaseType.Name == "Controller").ToList();
                List<string> errors = new List<string>();
                
                foreach(Type controller in types)
                {
                    try
                    {
                        ezActionsScript.AppendLine(controller.Name);

                        foreach (MethodInfo mi in controller.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(p => p.IsSpecialName == false && !typeof(object).GetMethods().Select(me => me.Name).Contains(p.Name) && p.Name != "Dispose"))
                        {
                            ezActionsScript.AppendLine("- " + mi.Name + "(" + (mi.GetParameters() != null ? string.Join(",", mi.GetParameters().Select(p => p.Name)) : "None") + "), Return: " + mi.ReturnType.Name);
                        }
                    }
                    catch(Exception e)
                    {
                        errors.Add(controller.Name + " failed to generate because of following error: " + e.Message + ", " + e.StackTrace);
                    }
                }

                if(errors.Count > 0)
                {
                    File.WriteAllText(solutionDir + "/Errors.txt", string.Join("\n", errors));
                }

                File.WriteAllText(solutionDir + "/EZActions.js", ezActionsScript.ToString());


                ShowMessage("Generated successfully!");
            }
            catch(Exception e)
            {
                ShowMessage("An error occured while generating: " + e.Message + ", " + e.StackTrace, OLEMSGICON.OLEMSGICON_CRITICAL);
            }
        }

        private void ShowMessage(string message, OLEMSGICON icon = OLEMSGICON.OLEMSGICON_INFO)
        {
            try
            {
                VsShellUtilities.ShowMessageBox(
                    provider,
                    message,
                    "Message from EZActions:",
                    icon,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
            catch { }
        }
    }
}
