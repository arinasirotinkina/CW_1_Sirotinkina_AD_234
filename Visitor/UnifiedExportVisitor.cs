using CW_1_Sirotinkina_AD_234.Domain;
using CW_1_Sirotinkina_AD_234.Interfaces;

namespace CW_1_Sirotinkina_AD_234.Visitor;

public class UnifiedExportVisitor : IVisitor
{
    public List<BankAccount> Accounts { get; } = new List<BankAccount>();
    public List<Category> Categories { get; } = new List<Category>();
    public List<Operation> Operations { get; } = new List<Operation>();

    public void Visit(BankAccount account)
    {
        Accounts.Add(account);
    }

    public void Visit(Category category)
    {
        Categories.Add(category);
    }

    public void Visit(Operation operation)
    {
        Operations.Add(operation);
    }
}