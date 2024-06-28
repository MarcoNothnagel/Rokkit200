using exceptions;
using static rokkit.SystemDB;

namespace rokkit
{
    public class TransactionHandler
    {
        public void ExecuteCode(int inputAccNum, string inputType, double inputAmount, SystemDB db, Transaction newTransaction)
        {
            var accounts = db.GetAccounts();

            try
            {
                Account? activeAccount = accounts.FirstOrDefault(account => account.CustomerNum == inputAccNum);

                if (activeAccount != null)
                {
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
                else
                {
                    throw new AccountNotFoundException($"Account with Customer Number {inputAccNum} not found.");
                }
            }
            catch (AccountNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}