using exceptions;
using rokkit;
using static rokkit.SystemDB;

public class Transaction : IAccountService
{
    public void Deposit(Account account, double amountToDeposit, SystemDB db)
    {
        Console.WriteLine("Deposit commencing");

        account.Balance += amountToDeposit;
        db.UpdateAccount(account);

        //Console.WriteLine($"Deposit of {amountToDeposit} successful. New balance: {account.Balance}");
    }

    public void OpenCurrentAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public void OpenSavingsAccount(int accountId, int amountToDeposit)
    {
        throw new NotImplementedException();
    }

    // Yes, I changed the interface to accept the account object because I am gangster like that
    public void Withdraw(Account account, double amountToWithdraw, SystemDB db)
    {
        Console.WriteLine("Withdrawal commencing");

        if (account.AccountType == AccountType.SAVINGS)
        {
            if ((account.Balance - amountToWithdraw) < 1000)
            {
                throw new WithdrawalAmountTooLargeException("Savings account needs to have more than R1000.00. Withdrawal limits reached.");
            }

        }
        else if (account.AccountType == AccountType.CURRENT)
        {
            if ((account.Balance + account.Overdraft) < amountToWithdraw)
            {
                throw new WithdrawalAmountTooLargeException("Current account withdrawal limits reached. Overdraft too small for transaction.");
            }
        }

        account.Balance -= amountToWithdraw;
        db.UpdateAccount(account);
        //Console.WriteLine($"Withdrawal of {amountToWithdraw} successful. New balance: {account.Balance}");

    }
}