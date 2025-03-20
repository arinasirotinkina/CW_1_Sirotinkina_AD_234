using CW_1_Sirotinkina_AD_234.Interfaces;

namespace CW_1_Sirotinkina_AD_234.Domain;

public class BankAccount : IVisitable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public event EventHandler OnBalanceChanged;

    private decimal _balance;
    public decimal Balance
    {
        get => _balance;
        set
        {
            if (_balance != value)
            {
                _balance = value;
                OnBalanceChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    public BankAccount(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Balance = 0;
    }
    
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}