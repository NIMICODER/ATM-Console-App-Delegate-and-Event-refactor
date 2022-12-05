using ATM_Console_App1.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_Console_App1.Domain.CardUser
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public long UserBankAccountId  { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public Decimal TransactionAmount { get; set; } 
    }
}
