using CW_1_Sirotinkina_AD_234.Domain;

namespace CW_1_Sirotinkina_AD_234.Factories;

public static class DomainFactory
{
    public static BankAccount CreateBankAccount(string name)
    {
        return new BankAccount(name);
    }

    public static Category CreateCategory(Category.CategoryType type, string name)
    {
        return new Category(type, name);
    }

    public static Operation CreateOperation(Operation.OperationType type, BankAccount account, decimal amount,
        DateTime date, string description, Category category)
    {
        return new Operation(type, account, amount, date, description, category);
    }
}