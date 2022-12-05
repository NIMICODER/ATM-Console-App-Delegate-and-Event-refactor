using ATM_Console_App.UI;
using ATM_Console_App1.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.App
{
    class Deployment
    {
        static void Main(string[] args)
        {
           
            ATMApp atmApp = new ATMApp();
            atmApp.InitializeData();
            atmApp.Run();

          
            //long cardNumber = Validate.Convert<long>("your card number");
            //Console.WriteLine($"Your card number is {cardNumber}");
            // Utility.PressEnterToContinue(); 

        }
    }
}
