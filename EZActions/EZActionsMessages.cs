using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZActions
{
    class EZActionsMessages
    {
        //Warning messages
        public static string EZ_MESSAGE_WARNING_PROJECT_NOT_OPEN = "It doesn't seem like you have any MVC Project open.\r\n\r\nPlease open an MVC Project, then try again!";
        public static string EZ_MESSAGE_WARNING_JS_FILE_PATH_NOT_SPECIFIED = "To generate the EZActions JavaScript File you need to have set up the JavaScript File Path in the Options.\r\n\r\nYou either haven't specified this or the path is invalid.\r\n\r\nGo to 'Tools -> Options -> EZActions -> General' and then enter the path to a valid Directory where the JavaScript File should be generated.";

        //Error messages
        public static string EZ_MESSAGE_ERROR_GENERATE_CONTROLLER = "{0} failed to generate because of following error: {1}, {2}";
        public static string EZ_MESSAGE_ERROR_OTHERS = "An error occured while generating:\r\n\r\n{0}\r\n\r\n{1}";

        //Successful messages
        public static string EZ_MESSAGE_SUCCESS_GENERATED_JS_FILE = "Generated successfully at the location:\r\n\r\n{0}";
        public static string EZ_MESSAGE_SUCCESS_GENERATED_ERROR_JS_FILE = "Almost generated successfully at the location:\r\n\r\n{0}\r\n\r\nOne or more errors occured doing the controller generation. To see the errors, check the file 'ezactions_error.txt' in the root of your project.";

        //Miscellaneous messages
        public static string EZ_MESSAGE_MISC_MSG_FROM_EZACTIONS = "Message from EZActions:";

        //File names
        public static string EZ_MESSAGE_FILENAME_ERRORS_FILE = "ezactions_errors.txt";
    }
}
