using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EZActions
{
    class EZActionsBuilder
    {
        private List<Type> controllers;

        public EZActionsBuilder(List<Type> controllers)
        {
            this.controllers = controllers;
        }

        public string Build()
        {
            try
            {
                string ClassTemplate = EZActionsBuilderTemplates.ClassTemplate;
                string ControllerTemplate = EZActionsBuilderTemplates.ControllerTemplate;
                string ActionTemplate = EZActionsBuilderTemplates.ActionTemplate;

                StringBuilder Controllers = new StringBuilder();
                int controllersCounter = 0;

                foreach(Type controller in controllers)
                {
                    try
                    {
                        string ControllerName = controller.Name.Replace("Controller", "");
                        string _controller = ControllerTemplate;
                        StringBuilder Actions = new StringBuilder();

                        List<MethodInfo> methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(p => p.IsSpecialName == false && !typeof(object).GetMethods().Select(me => me.Name).Contains(p.Name) && p.Name != "Dispose").ToList();
                        int methodsCounter = 0;

                        foreach (MethodInfo action in methods)
                        {
                            string _action = ActionTemplate;
                            StringBuilder actionParameters = new StringBuilder();
                            StringBuilder dataParameters = new StringBuilder();

                            _action = _action.Replace("{A_NAME}", action.Name);
                            _action = _action.Replace("{A_URL}", "/" + ControllerName + "/" + action.Name);

                            ParameterInfo[] parameters = action.GetParameters();

                            foreach (ParameterInfo parameter in parameters)
                            {
                                if(parameter == parameters.Last())
                                {
                                    actionParameters.Append(parameter.Name + ", ");
                                    dataParameters.Append(parameter.Name + ":" + parameter.Name);
                                }
                                else
                                {
                                    actionParameters.Append(parameter.Name + ", ");
                                    dataParameters.Append(parameter.Name + ":" + parameter.Name + ", ");
                                }
                            }

                            _action = _action.Replace("{A_PARS}", actionParameters.ToString());
                            _action = _action.Replace("{A_DATA}", dataParameters.ToString());

                            if(methodsCounter == methods.Count - 1)
                            {
                                Actions.AppendLine(_action);
                            }
                            else
                            {
                                Actions.AppendLine(_action + ",");
                            }
                            methodsCounter++;
                        }

                        _controller = _controller.Replace("{C_NAME}", ControllerName);
                        _controller = _controller.Replace("{C_FUNCTIONS}", Actions.ToString());

                        if(controllersCounter == controllers.Count - 1)
                        {
                            Controllers.AppendLine(_controller);
                        }
                        else
                        {
                            Controllers.AppendLine(_controller + ",");
                        }
                        controllersCounter++;
                    }
                    catch { }
                }

                string classObj = ClassTemplate;

                classObj = classObj.Replace("{C_CONTROLLERS}", Controllers.ToString());

                return classObj;
            }
            catch
            {
                return null;
            }
        }
    }

    class EZActionsBuilderTemplates
    {

        public static string ClassTemplate =
        @"
            var EZActions = new EZActionsJS();

            function EZActionsJS() {
                {C_CONTROLLERS}
            }
        ";

        public static string ControllerTemplate =
        @"
            this.{C_NAME} = {
                {C_FUNCTIONS}
            }
        ";

        public static string ActionTemplate =
        @"
            {A_NAME}: function ({A_PARS}sc = 0, ec = 0) {
                ec = ec || 0;

                $.ajax({
                    url: '{A_URL}',
                    type: 'GET',
                    data: {
					    {A_DATA}
                    },
                    success: function(data) {
                        if(sc != 0) {
                            sc(data);
                        }
                    }, error: function(e) {
                        if(ec != 0) {
                            ec(e);
                        }
                    }
                });
            }
        ";
    }

}
