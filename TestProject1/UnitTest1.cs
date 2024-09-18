using System.ComponentModel;
using RefactoringExample;

namespace TestProject1;

public class UnitTest1
{
    [Theory]
    [InlineData(true, 100, 100, 100, true, "Remove all money from Checking")]
    [InlineData(false,100, 50, 100,  false, "Not enough money in checking, no overdraft")]
    [InlineData(true, 50, 100, 100,  true, "Not enough money in checking, has overdraft")]
    [InlineData(false, 100, 10, 89, true, "Not enough money in checking, not enough in savings to cover the overdraft")]
    public void WithdrawFromChecking(bool expectedResult, decimal amount, decimal checkingBalance, decimal savingsBalance, bool hasOverdraftProtection, string testName)
    {
        // Arrange
        var checking = new Account { Balance = checkingBalance, OverdraftProtection = hasOverdraftProtection };
        var savings = new Account { Balance = savingsBalance };

        // Act
        var result = OnlineBankingService.WithdrawFromChecking(amount, checking, savings);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}