using System;
using static rokkit.SystemDB;

namespace rokkit
{


    // fixed the naming format
    public interface IAccountService
    {
        public void OpenSavingsAccount(int accountId, int amountToDeposit);
        public void OpenCurrentAccount(int accountId);
        public void Withdraw(Account account, double amountToWithdraw, SystemDB db);
        //throws AccountNotFoundException, WithdrawalAmountTooLargeException;
        public void Deposit(Account account, double amountToWithdraw, SystemDB db);
        // throws AccountNotFoundException;
    }
}