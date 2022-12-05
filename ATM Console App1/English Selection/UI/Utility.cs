using System; 
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.UI
{
    public static class Utility
    {
        private static long tranId;
        private static CultureInfo culture = new CultureInfo("en-GB");  

        public static long GetTrantionId()
        {
             return ++tranId;
        }
        public static string GetSecetInput(string prompt)
        {
            bool isPrompt = true;
            string asterics = "";

            StringBuilder input = new StringBuilder();  
            while (true)
            {
                if (isPrompt)
                    Console.WriteLine(prompt);
                isPrompt = false;
               ConsoleKeyInfo inputKey =  Console.ReadKey(true);
                if (inputKey.Key == ConsoleKey.Enter)
                {
                    if (input.Length == 4)
                    {
                        break;
                    }
                    else
                    {
                        Displaymsg("\nPlease enter 4 digits", false);
                        input.Clear();
                        isPrompt = true;
                        continue;
                        
                    }
                }

                if (inputKey.Key == ConsoleKey.Backspace && input.Length > 0) 
                {
                    input.Remove(input.Length - 1, 1);
                }
                else if (inputKey.Key != ConsoleKey.Backspace)
                {
                    input.Append(inputKey.KeyChar);
                    Console.Write($"{asterics}*");
                }
            }
            return input.ToString();

        }
        public static void Displaymsg(string msg, bool success)
        {
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red; 
            }
            Console.WriteLine(msg);
            Console.ForegroundColor= ConsoleColor.White;
            PressEnterToContinue();
        }
        public static string GetUserInput(string prompt)
        {
            Console.WriteLine($"Enter {prompt}");
            return Console.ReadLine();
        }

        public static void DisplayDotAnimation(int timer = 10)
        {
            for (int i = 0; i < timer; i++)
            {
                Console.Write(".");
                Thread.Sleep(200);
                //tempUserAccount.CardNumber = i;
            }
            Console.Clear();
        }
        public static void PressEnterToContinue()
        {
            Console.WriteLine("\nPress Enter to Continue...\n");
            Console.ReadLine();
        }
        public static string FormatAmount(decimal amt)
        {
            return string.Format(culture, "{0:C2}", amt);
        }
    }
}
