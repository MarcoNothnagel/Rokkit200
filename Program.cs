using System;
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using exceptions;
using static SystemDB;

namespace rokkit
{
  class Program
  {
    static void Main(string[] args)
    {

      int inputAccNum = 4;
      string inputType = "Withdraw";
      double inputAmount = 10000.00;


      var db = SystemDB.Instance;
      Transaction newTransaction = new();
      var accounts = db.GetAccounts();

      Account activeAccount = accounts.FirstOrDefault(account => account.CustomerNum == inputAccNum);
      Console.WriteLine($"Found active account: Customer number {activeAccount.CustomerNum}, Account type: {activeAccount.AccountType}, Balance: {activeAccount.Balance}");

      if (inputType == "Withdraw")
      {
        try
        {
          newTransaction.Withdraw(activeAccount, inputAmount, db);
        }
        catch (WithdrawalAmountTooLargeException ex)
        {
          Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Unexpected error: {ex.Message}");
        }
      }
      else if (inputType == "Deposit")
      {
        newTransaction.Deposit(activeAccount, inputAmount, db);
      }

    }
  }

}



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



// The master plan TODO list:

// Step 1: get a rough draft working ---> done
// Step 2: make the code look nice
// Step 3: testing
// Step 4: logging
// Step 5: API?