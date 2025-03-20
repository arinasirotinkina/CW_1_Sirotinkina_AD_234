using CW_1_Sirotinkina_AD_234.Domain;

namespace CW_1_Sirotinkina_AD_234.Infrastructure;

public interface IFinanceRepository
{
    void AddBankAccount(BankAccount bankAccount);
    List<BankAccount> GetBankAccounts();
    void UpdateBankAccount(BankAccount bankAccount);
    void RemoveBankAccount(Guid bankId);
    
    void AddCategory(Category category);
    List<Category> GetCategories();
    void UpdateCategory(Category category);
    void RemoveCategory(Guid categoryId);
    
    void AddOperation(Operation operation);
    List<Operation> GetOperations();
    void UpdateOperation(Operation operation);
    void RemoveOperation(Guid operationId);
}