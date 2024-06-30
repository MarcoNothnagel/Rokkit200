using exceptions;
using static rokkit.SystemDB;

namespace rokkit
{

    public sealed class SystemDB
    {
        private static readonly SystemDB instance = new SystemDB();

        public class Account
        {
            public int CustomerNum { get; set; }
            public double Balance { get; set; }
            public AccountType AccountType { get; set; }
            public double Overdraft { get; set; }

            public Account(int customerNum, AccountType accountType, double balance, double overdraft = 0)
            {
                CustomerNum = customerNum;
                AccountType = accountType;
                Balance = balance;
                Overdraft = overdraft;
            }
        }

        public enum AccountType
        {
            SAVINGS,
            CURRENT
        }
        private readonly HashSet<Account> accounts;

        private SystemDB()
        {

            accounts = new HashSet<Account>()
    {
      new(1, AccountType.SAVINGS, 2000),
      new(2, AccountType.SAVINGS, 5000),
      new(3, AccountType.CURRENT, 1000, overdraft: 10000),
      new(4, AccountType.CURRENT, -5000, overdraft: 20000)
    };
        }

        public static SystemDB Instance
        {
            get { return instance; }
        }

        public IEnumerable<Account> GetAccounts()
        {
            // Return a copy of the set to avoid modifying the internal collection
            return new HashSet<Account>(accounts);
        }

        public void UpdateAccount(Account updatedAccount)
        {
            // Find the account in the HashSet and update it
            var accountToUpdate = accounts.FirstOrDefault(a => a.CustomerNum == updatedAccount.CustomerNum);
            if (accountToUpdate != null)
            {
                accountToUpdate.Balance = updatedAccount.Balance;
                //Console.WriteLine($"Withdrawal of {amountToWithdraw} successful. New balance: {account.Balance}");
            }
            else
            {
                throw new AccountNotFoundException($"Account with Customer Number {updatedAccount.CustomerNum} not found in SystemDB.");
            }

            foreach (var account in accounts)
            {
                Console.WriteLine($"Customer number: {account.CustomerNum}, Account type: {account.AccountType}, Balance: {account.Balance}, Overdraft: {account.Overdraft}");
            }
        }
    }

}