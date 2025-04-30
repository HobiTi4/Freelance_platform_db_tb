using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance_Platform.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        public DateTime Date { get; set; }
    }
    public enum TransactionType
    {
        Deposit,
        Payment,
        Withdrawal
    }

}
