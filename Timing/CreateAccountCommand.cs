using CW_1_Sirotinkina_AD_234.Facade;

namespace CW_1_Sirotinkina_AD_234.Timing;

public class CreateAccountCommand : ICommand
{
    private readonly FinanceFacade _facade;
    private readonly string _accountName;

    public CreateAccountCommand(FinanceFacade facade, string accountName)
    {
        _facade = facade;
        _accountName = accountName;
    }

    public void Execute()
    {
        var account = _facade.CreateBankAccount(_accountName);
        Console.WriteLine($"Создан аккаунт: {account.Id} - {account.Name}.");
    }
}