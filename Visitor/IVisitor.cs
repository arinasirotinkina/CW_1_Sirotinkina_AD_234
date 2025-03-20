using CW_1_Sirotinkina_AD_234.Domain;

namespace CW_1_Sirotinkina_AD_234.Interfaces;

public interface IVisitor
{
    void Visit(BankAccount account);
    void Visit(Operation operation);
    void Visit(Category category);
}