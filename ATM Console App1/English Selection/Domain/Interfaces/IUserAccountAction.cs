using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.Domain.Interfaces
{
    internal interface IUserAccountActions
    {
        void CheckBalance();
        void MakeDeposit();
        void MakeWithdrawal();
    }
}
