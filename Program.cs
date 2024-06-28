using exceptions;
using static rokkit.SystemDB;

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

      TransactionHandler handler = new TransactionHandler();

      handler.ExecuteCode(inputAccNum, inputType, inputAmount, db, newTransaction);

    }

  }

}




// The master plan TODO list:

// Step 1: get a rough draft working ---> done
// Step 2: make the code look nice ---> done
// Step 3: testing
// Step 4: logging
// Step 5: API?