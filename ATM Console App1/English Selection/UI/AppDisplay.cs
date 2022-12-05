using ATM_Console_App1.Domain.CardUser;
using ATM_Console_App1.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App.UI
{
    public  class AppDisplay
    {
        internal const string cur = "N ";
       // public static int cur;

        internal static void Welcome()
        {
            // Console.WriteLine("Hello, World!"); 
            Console.Clear();
            Console.Title = "Genesys ATM App";
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n\n------ Welcome to Genesys Bank-----\n\n");

            //prompt user to insert card
            Console.WriteLine("Please insert your card");

           // Utility.PressEnterToContinue();
        }
        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validate.Convert<long>("your card number:");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecetInput("Enter your card Pin:"));

            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.DisplayDotAnimation();
        }

        internal static void DisplayLockmsg()
        {
            Console.Clear();
            Utility.Displaymsg("Your account is locked. Please go to the nearest branch to unlock your account. Thank you.", true);
            Utility.PressEnterToContinue();
            //AppDisplay.DisplayAppMenu();
        }

        internal static void WelcomeUser(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("----Welcome to Genesys Bank----");
            Console.WriteLine(":                             :");
            Console.WriteLine("1. Check Balance              :");
            Console.WriteLine("2. Cash Deposit               :");
            Console.WriteLine("3. Withdrawal                 :");
            Console.WriteLine("4. Transfer                   :");
            Console.WriteLine("5. Transactions               :");
            Console.WriteLine("6. Logout                     :");
        }

        internal static void LogoutProgress()
        {
            Console.WriteLine("Thank you for Banking with us");
            Utility.DisplayDotAnimation();
            Console.Clear();
        }

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}500     3.{0}5000", cur);
            Console.WriteLine(":2.{0}1000    4.{0}10000", cur);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectedAmount = Validate.Convert<int>("option");
            switch (selectedAmount)
            {
                case 1:
                    return 500;
                    break;
                case 2:
                    return 1000;
                    break;
                case 3:
                    return 5000;
                    break;
                case 4:
                    return 10000;
                    break;
                case 0:
                    return 0;
                    break;
                 default:
                    Utility.Displaymsg("Invalid input. Try again", false);
                    return -1;
                    break;

            }
        }
        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.RecipientBankAccountNumber = Validate.Convert<long>("recipients account number:");
            internalTransfer.TransferAmount = Validate.Convert<decimal>($"amount {cur}");
            internalTransfer.RecipientBankAccountName = Utility.GetUserInput("recipient's Name: ");
            return internalTransfer;
            

        }
    }
}
