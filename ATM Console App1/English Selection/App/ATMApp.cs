using ATM_Console_App.UI;
using ATM_Console_App1.Domain.CardUser;
using ATM_Console_App1.Domain.Enums;
using ATM_Console_App1.Domain.Interfaces;
using ATM_Console_App1.UI;
using ConsoleTables;
using System.Linq;

namespace ATM_Console_App1.App
{
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {
        private List<UserAccount>? userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _ListOfTransactions;
        private const decimal minimumBalance = 1000;
        private readonly AppDisplay display;

        public ATMApp()
        {
            display = new AppDisplay();
        }
        public void Run()
        {
            AppDisplay.Welcome();
            CheckUserCardNumAndPassword();
            AppDisplay.WelcomeUser(selectedAccount.FullName);
            while(true) { 
            AppDisplay.DisplayAppMenu();
            ProcessMenuOption();
            }
            //AppDisplay.DisplayAppMenu();
            //ProcessMenuOption();
        }

        public void InitializeData()
        {
             userAccountList = new List<UserAccount>
            {
                new UserAccount{ Id = 1, FullName = "David Ukpoju", AccountNumber = 12345, CardNumber = 123456, CardPin = 1234, AccountBalance = 50000.00m,IsLocked = false },
                new UserAccount{ Id = 2, FullName = "Alex Ogubuike", AccountNumber = 54321, CardNumber = 654321,CardPin = 4321, AccountBalance = 100000.00m,IsLocked = false },
                new UserAccount{ Id = 1, FullName = "Chidiebube Onah", AccountNumber = 34512, CardNumber = 456123,CardPin = 3412, AccountBalance = 75000.00m,IsLocked = true },
            };

            _ListOfTransactions = new List<Transaction>();
        }

        public void CheckUserCardNumAndPassword()
        {
            bool isCorrectLogin = false;
            while (isCorrectLogin == false)
            {
                UserAccount inputAccount = AppDisplay.UserLoginForm();
                AppDisplay.LoginProgress();
                foreach (UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if (inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if (inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if (selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                //display a lock message
                                AppDisplay.DisplayLockmsg();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (isCorrectLogin == false)
                    {
                        Utility.Displaymsg("\n Invalid card number or PIN", false);
                        selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                        if (selectedAccount.IsLocked)
                        {
                            AppDisplay.DisplayLockmsg();
                        }
                    }
                    Console.Clear();

                }
            }

        }

        private void ProcessMenuOption()
        {
            switch (Validate.Convert<int>("an option:"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.MakeDeposit:
                    MakeDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
;                    break;
                case (int)AppMenu.MakeTransfer:
                    var internalTransfer = display.InternalTransferForm();
                    ProcessTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransaction();
                    break;
                case (int)AppMenu.Logout:
                    AppDisplay.LogoutProgress();
                    Utility.Displaymsg("You have Logged out Successfully. Please collect your ATM card ", true);
                    Run();
                    break;
                default:
                    Utility.Displaymsg("Invalid Option", false);
                    break;
            }
        }

        public void CheckBalance()
        {
            Utility.Displaymsg($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}", true);
        }

        public void MakeDeposit()
        {
            Console.WriteLine(" \nOnly multiples of 500 and 1000 allowed");
            var transaction_amt = Validate.Convert<int>($"amount {AppDisplay.cur}");

            //SIMULATE COUNTING
            Console.WriteLine("\nChecking and Counting bank notes.");
            Utility.DisplayDotAnimation();
            Console.WriteLine("");

            //some guard clause
            if (transaction_amt <= 0)
            {
                Utility.Displaymsg("Amount needs to be greater than Zero. Try again.", false);
                return;
            }
            if (transaction_amt % 500 != 0)
            {
                Utility.Displaymsg("Enter Amount in multiple of 500 or 1000. Try again.", false);
                return;
            }
            if (PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.Displaymsg($" You have cancelled your action.", false);
                return;
            }

            //bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Deposit, transaction_amt, "");

            //update account balace
            selectedAccount.AccountBalance += transaction_amt;

            //Display success message
            Utility.Displaymsg($"Your deposit of {Utility.FormatAmount(transaction_amt)} was successful.", true);



        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppDisplay.SelectAmount();
            if (selectedAmount == -1)
            {
                MakeWithdrawal();
                return;
            }
            else if (selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validate.Convert<int>($"amount {AppDisplay.cur}");
            }

            //input validation
            if (transaction_amt <= 0)
            {
                Utility.Displaymsg("Amount needs to be greater than zero. Try again", false);
                return;
            }

            if (transaction_amt % 500 != 0)
            {
                Utility.Displaymsg("You can only withdraw amount in multiples of 500 or 1000 naira. Try again.", false);
                return;
            }

            //business logic validations
            if (transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.Displaymsg($"Withdrawal failed. Insufficient Funds! {Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if ((selectedAccount.AccountBalance - transaction_amt) < minimumBalance)
            {
                Utility.Displaymsg($"Withdrawaw failed. You must have minimum {Utility.FormatAmount(minimumBalance)}", false);
                return;
            }

            //bind withdrawal details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
            //update account balance
            selectedAccount.AccountBalance -= transaction_amt;
            //success msg
            Utility.Displaymsg($"You have successfully withdrawn {Utility.FormatAmount(transaction_amt)}.", true);
        }
        private bool PreviewBankNotesCount(int amount)
        {
            int thousandNotesCount = amount / 1000;
            int fiveHundredNotesCount = (amount % 1000) / 500;

            Console.WriteLine("\nSummary");
            Console.WriteLine("------");
            Console.WriteLine($"{AppDisplay.cur} 1000 X {thousandNotesCount} = {1000 * thousandNotesCount} "); 
            Console.WriteLine($"{AppDisplay.cur}500 X {fiveHundredNotesCount} = {500 * fiveHundredNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");
            int opt = Validate.Convert<int>("1 to confirm");
            return opt.Equals(1);

        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            //creating a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTrantionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };

            //add transaction object to the list
            _ListOfTransactions.Add(transaction);


        }

        public void ViewTransaction()
        {
           var filteredTransactionList = _ListOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();
           //check if there's a transaction
           if(filteredTransactionList.Count <= 0)
            {
                Utility.Displaymsg("You have no transaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date", "Type", "Descriptions", "Amount" + AppDisplay.cur);
                foreach(var trans in filteredTransactionList)
                {
                    table.AddRow(trans.TransactionId, trans.TransactionDate,trans.TransactionType, trans.Description, trans.TransactionAmount);
                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.Displaymsg($"you have {filteredTransactionList.Count} transaction(s)", true);
            }
        }

        
        private void ProcessTransfer(InternalTransfer internalTransfer)
        {
            if (internalTransfer.TransferAmount <= 0)
            {
                Utility.Displaymsg("Amount needs to be more than zero. Try again.", false);
                return;
            }

            //check senders account balance
            if (internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.Displaymsg($"Transfer failed. You do not have enough balance to internalTransfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return;
            }
            //check minimum balane
            if ((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumBalance)
            {
                Utility.Displaymsg($"Transfer failed. your account needs to have minimum {Utility.FormatAmount(minimumBalance)}", false);
                return;
            }


            //checked if recievers account is valid
            var selectedBankAccountReciever = (from userAcc in userAccountList
                                        where userAcc.AccountNumber == internalTransfer.RecipientBankAccountNumber
                                        select userAcc).FirstOrDefault();
            if (selectedBankAccountReciever == null)
            {
                Utility.Displaymsg("Transfer failed. Reciever bank account is invalid.", false);
            }

            //check recievers name
            if (selectedBankAccountReciever.FullName != internalTransfer.RecipientBankAccountName)
             {
                 Utility.Displaymsg("Transfer failed. recipients bank account does not match.", false);
             }
            //add transaction to transaction record- sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, $"Transfered to {selectedBankAccountReciever.AccountNumber} ({selectedBankAccountReciever.FullName})");
            // update senders account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;
            //add transaction record-reciever
            InsertTransaction(selectedBankAccountReciever.Id, TransactionType.Transfer, internalTransfer.TransferAmount, $"Transfered from { selectedAccount.AccountNumber} ({ selectedAccount.FullName}");
            //update recievers account balance
            selectedBankAccountReciever.AccountBalance += internalTransfer.TransferAmount;
            //display success
            Utility.Displaymsg($"You have successfully Transfered {Utility.FormatAmount(internalTransfer.TransferAmount)} to {internalTransfer.RecipientBankAccountName}", true);
        }


    }
}