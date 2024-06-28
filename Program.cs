using System;
using exceptions;
using static SystemDB;

namespace rokkit
{
  class Program
  {
    static void Main(string[] args)
    {
      Transaction newTransaction = new();

      int inputAccNum = 4;
      string inputType = "Deposit";
      double inputAmount = 10000.00;


      var db = SystemDB.Instance;
      var accounts = db.GetAccounts();

      Account activeAccount = accounts.FirstOrDefault(account => account.CustomerNum == inputAccNum);
      Console.WriteLine($"Found active account: Customer number {activeAccount.CustomerNum}, Account type: {activeAccount.AccountType}, Balance: {activeAccount.Balance}");

      if (inputType == "Withdraw")
      {
        try
        {
          newTransaction.Withdraw(activeAccount, inputAmount);
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
        newTransaction.Deposit(activeAccount, inputAmount);
      }

    }
  }
}


public class Transaction : IAccountService
{
  public void Deposit(Account account, double amountToDeposit)
  {
    Console.WriteLine("Withdrawal commencing");

    account.Balance += amountToDeposit;
    Console.WriteLine($"Deposit of {amountToDeposit} successful. New balance: {account.Balance}");
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
  public void Withdraw(Account account, double amountToWithdraw)
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
    Console.WriteLine($"Withdrawal of {amountToWithdraw} successful. New balance: {account.Balance}");

  }
}


// fixed the naming format
public interface IAccountService
{
  public void OpenSavingsAccount(int accountId, int amountToDeposit);
  public void OpenCurrentAccount(int accountId);
  public void Withdraw(Account account, double amountToWithdraw);
  //throws AccountNotFoundException, WithdrawalAmountTooLargeException;
  public void Deposit(Account account, double amountToWithdraw);
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

  // No need for add, delete, or update methods as requested

  public IEnumerable<Account> GetAccounts()
  {
    // Return a copy of the set to avoid modifying the internal collection
    return new HashSet<Account>(accounts);
  }
}



// The master plan TODO list:

// Step 1: get a rough draft working ---> done
// Step 2: make the code look nice
// Step 3: testing
// Step 4: logging
// Step 5: API?