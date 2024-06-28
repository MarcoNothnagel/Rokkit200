namespace exceptions;

public class WithdrawalAmountTooLargeException : Exception
{
    public WithdrawalAmountTooLargeException() : base() { }
    public WithdrawalAmountTooLargeException(string message) : base(message) { }
    public WithdrawalAmountTooLargeException(string message, Exception innerException) : base(message, innerException) { }
}


public class AccountNotFoundException : Exception
{
    public AccountNotFoundException() : base() { }
    public AccountNotFoundException(string message) : base(message) { }
    public AccountNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
