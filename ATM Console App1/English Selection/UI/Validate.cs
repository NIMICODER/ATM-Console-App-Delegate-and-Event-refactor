using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.UI
{
    public static class Validate
    {
        //a generic method(T is the type input)
        public static T Convert<T>(string prompt)
        {
            bool valid = false;
            string userInput;
            while (!valid)
            {
                userInput = Utility.GetUserInput(prompt);
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T)converter.ConvertFromString(userInput);
                    } else
                    {
                        return default(T);
                    }
                }catch
                {
                    Utility.Displaymsg("Invalid input. Try again..", false);
                }
            }
            return default;
        }
    }
}
