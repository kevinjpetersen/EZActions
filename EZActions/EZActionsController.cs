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
        private Package package;

        public EZActionsController(IServiceProvider provider, Package package)
        {
            this.provider = provider;
            this.package = package;
        }

        public void Generate()
        {
            try
            {
                OptionPageGrid ezSettings = (OptionPageGrid)this.package.GetDialogPage(typeof(OptionPageGrid));

                if(provider == null)
                {
                    ShowMessage(EZActionsMessages.EZ_MESSAGE_WARNING_PROJECT_NOT_OPEN, OLEMSGICON.OLEMSGICON_WARNING);
                    return;
                }

                DTE dte = (DTE)provider.GetService(typeof(DTE));

                if(dte == null || dte.Solution == null || dte.Solution.IsOpen == false)
                {
                    ShowMessage(EZActionsMessages.EZ_MESSAGE_WARNING_PROJECT_NOT_OPEN, OLEMSGICON.OLEMSGICON_WARNING);
                    return;
                }

                if (string.IsNullOrEmpty(ezSettings.OptionEZActionsJSFilePath) || Directory.Exists(ezSettings.OptionEZActionsJSFilePath) == false)
                {
                    ShowMessage(EZActionsMessages.EZ_MESSAGE_WARNING_JS_FILE_PATH_NOT_SPECIFIED, OLEMSGICON.OLEMSGICON_WARNING);
                    return;
                }

                string solutionDir = System.IO.Path.GetDirectoryName(dte.Solution.FullName);
                StringBuilder ezActionsScript = new StringBuilder();

                List<Type> types = EZActionsUtilities.GetAllTypes(provider).Where(p => p.BaseType != null && p.BaseType.Name == "Controller").ToList();
                List<string> errors = new List<string>();

      
                // TODO: Notify about errors when generating/building the script

                try
                {
                    EZActionsBuilder ezBuilder = new EZActionsBuilder(types);
                    string script = ezBuilder.Build();

                    if(string.IsNullOrEmpty(script))
                    {
                        ShowMessage("Error happened when building script!");
                        return;
                    }

                    ezActionsScript.Append(script);
                }
                catch //(Exception e)
                {
                    //errors.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + string.Format(EZActionsMessages.EZ_MESSAGE_ERROR_GENERATE_CONTROLLER, controller.Name, e.Message, e.StackTrace) + "\r\n");
                }
            

                File.WriteAllText(ezSettings.OptionEZActionsJSFilePath + "/" + ezSettings.OptionEZActionsJSFileName, ezActionsScript.ToString());

                if (errors.Count > 0)
                {
                    File.AppendAllText(solutionDir + "/" + EZActionsMessages.EZ_MESSAGE_FILENAME_ERRORS_FILE, string.Join("\n", errors));
                    ShowMessage(string.Format(EZActionsMessages.EZ_MESSAGE_SUCCESS_GENERATED_ERROR_JS_FILE, ezSettings.OptionEZActionsJSFilePath), OLEMSGICON.OLEMSGICON_WARNING);
                }
                else
                {
                    ShowMessage(string.Format(EZActionsMessages.EZ_MESSAGE_SUCCESS_GENERATED_JS_FILE, ezSettings.OptionEZActionsJSFilePath));
                }
            }
            catch(Exception e)
            {
                ShowMessage(string.Format(EZActionsMessages.EZ_MESSAGE_ERROR_OTHERS, e.Message, e.StackTrace), OLEMSGICON.OLEMSGICON_CRITICAL);
            }
        }

        private void ShowMessage(string message, OLEMSGICON icon = OLEMSGICON.OLEMSGICON_INFO)
        {
            try
            {
                VsShellUtilities.ShowMessageBox(
                    provider,
                    message,
                    EZActionsMessages.EZ_MESSAGE_MISC_MSG_FROM_EZACTIONS,
                    icon,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
            catch { }
        }
    }
}
