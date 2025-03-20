using CW_1_Sirotinkina_AD_234.Domain;

namespace CW_1_Sirotinkina_AD_234.Observer;

public class BalanceLogger : IBankAccountObserver
{
    public void HandleBalanceChanged(object sender, EventArgs e)
    {
        if (sender is BankAccount account)
        {
            Console.WriteLine($"[INFO] [Observer] Баланс счета {account.Name} обновлен: {account.Balance}.");
        }
    }
}