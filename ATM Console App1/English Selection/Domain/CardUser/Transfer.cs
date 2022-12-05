
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.Domain.CardUser
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }
        public long RecipientBankAccountNumber { get; set; }   
        public string RecipientBankAccountName { get; set; }

    }
}
