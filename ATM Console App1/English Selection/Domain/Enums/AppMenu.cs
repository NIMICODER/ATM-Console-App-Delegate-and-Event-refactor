using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.Domain.Enums
{
     public enum AppMenu
    {
        CheckBalance = 1,//0
        MakeDeposit,//1
        MakeWithdrawal,
        MakeTransfer,
        ViewTransaction,
        Logout
    }
}
