namespace RefactoringExample;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}

public class OnlineBankingService(SomeBankingServices bankingServices)
{
    // This method requires an integration test due to the dependency on other services.
    public bool CheckingWithdrawal_A(decimal amount, Guid checkingId, Guid savingsId)
    {
        var checking = bankingServices.GetCheckingAccount(checkingId);
        var savings = bankingServices.GetSavingsAccount(savingsId);

        if (checking.Balance >= amount)
        {
            checking.Balance -= amount;
            return true;
        }
        
        if (checking.OverdraftProtection)
        {
            var difference = amount - checking.Balance;
            if (savings.Balance >= difference)
            {
                checking.Balance = 0;
                savings.Balance -= difference;
                return true;
            }
        }

        return false;
    }

    // This method does the same thing as A and would require the same integration test;
    // however, the logic of was moved into a pure method so that it can be unit tested without the need for an integration test.
    public bool CheckingWithdrawal_B(decimal amount, Guid checkingId, Guid savingsId)
    {
        var checking = bankingServices.GetCheckingAccount(checkingId);
        var savings = bankingServices.GetSavingsAccount(savingsId);
        return WithdrawFromChecking(amount, checking, savings);
    }

    // Pure method that can be easily unit tested.
    public static bool WithdrawFromChecking(decimal amount, Account checking, Account savings)
    {
        if (checking.Balance >= amount)
        {
            checking.Balance -= amount;
            return true;
        }

        if (checking.OverdraftProtection)
        {
            var difference = amount - checking.Balance;
            if (savings.Balance >= difference)
            {
                checking.Balance = 0;
                savings.Balance -= difference;
                return true;
            }
        }

        return false;
    }
}

public class SomeBankingServices
{
    public Account GetCheckingAccount(Guid userId) => new();
    public Account GetSavingsAccount(Guid userId) => new();
}

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public decimal Balance { get; set; }
    public bool OverdraftProtection { get; set; }
}
