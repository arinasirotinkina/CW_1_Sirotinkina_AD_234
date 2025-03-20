using CW_1_Sirotinkina_AD_234.Interfaces;

namespace CW_1_Sirotinkina_AD_234.Domain;

public class Operation : IVisitable
{
    public enum OperationType { Income, Expense }
    
    public Guid Id { get; set; }
    public OperationType Type { get; set; }
    public BankAccount BankAccount { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public Category Category { get; set; }

    public Operation(OperationType type, BankAccount bankAccount, decimal amount, DateTime date, string description,
        Category category)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Число не может быть меньше 0");
        }
        
        Id = Guid.NewGuid();
        Type = type;
        BankAccount = bankAccount;
        Amount = amount;
        Date = date;
        Description = description;
        Category = category;
    }
    
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}