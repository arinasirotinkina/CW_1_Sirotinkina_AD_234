using System.Text;
using CW_1_Sirotinkina_AD_234.Interfaces;
using CW_1_Sirotinkina_AD_234.Visitor;

namespace CW_1_Sirotinkina_AD_234.DataExport;

public class YAMLDataExporter : DataExporter
{
    protected override IVisitor CreateVisitor()
    {
        return new UnifiedExportVisitor();
    }

    protected override string FormatData(IVisitor visitor)
    {
        var yamlVisitor = visitor as UnifiedExportVisitor;
        if (yamlVisitor == null)
            throw new InvalidOperationException("Неверный тип посетителя");

        StringBuilder sb = new StringBuilder();

        // Экспорт счетов
        sb.AppendLine("BankAccounts:");
        foreach (var account in yamlVisitor.Accounts)
        {
            sb.AppendLine("  -");
            sb.AppendLine($"    Id: {account.Id}");
            sb.AppendLine($"    Name: {account.Name}");
            sb.AppendLine($"    Balance: {account.Balance}");
        }

        // Экспорт категорий
        sb.AppendLine("Categories:");
        foreach (var category in yamlVisitor.Categories)
        {
            sb.AppendLine("  -");
            sb.AppendLine($"    Id: {category.Id}");
            sb.AppendLine($"    Type: {category.Type}");
            sb.AppendLine($"    Name: {category.Name}");
        }

        // Экспорт операций
        sb.AppendLine("Operations:");
        foreach (var op in yamlVisitor.Operations)
        {
            sb.AppendLine("  -");
            sb.AppendLine($"    Id: {op.Id}");
            sb.AppendLine($"    Type: {op.Type}");
            sb.AppendLine($"    BankAccountId: {op.BankAccount.Id}");
            sb.AppendLine($"    Amount: {op.Amount}");
            sb.AppendLine($"    Date: {op.Date:yyyy-MM-dd}");
            sb.AppendLine($"    Description: {op.Description}");
            sb.AppendLine($"    CategoryId: {op.Category.Id}");
        }

        return sb.ToString();
    }
}