using System.Text;
using CW_1_Sirotinkina_AD_234.Interfaces;
using CW_1_Sirotinkina_AD_234.Visitor;

namespace CW_1_Sirotinkina_AD_234.DataExport;

public class CSVDataExporter : DataExporter
{
    protected override IVisitor CreateVisitor()
    {
        return new UnifiedExportVisitor();
    }

    protected override string FormatData(IVisitor visitor)
    {
        var csvVisitor = visitor as UnifiedExportVisitor;
        if (csvVisitor == null)
            throw new InvalidOperationException("Неверный тип посетителя");

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("BankAccounts:");
        sb.AppendLine("Id,Name,Balance");
        foreach (var account in csvVisitor.Accounts)
        {
            sb.AppendLine($"{account.Id},{account.Name},{account.Balance}");
        }
        sb.AppendLine();

        sb.AppendLine("Categories:");
        sb.AppendLine("Id,Type,Name");
        foreach (var category in csvVisitor.Categories)
        {
            sb.AppendLine($"{category.Id},{category.Type},{category.Name}");
        }
        sb.AppendLine();

        sb.AppendLine("Operations:");
        sb.AppendLine("Id,Type,BankAccountId,Amount,Date,Description,CategoryId");
        foreach (var op in csvVisitor.Operations)
        {
            sb.AppendLine($"{op.Id},{op.Type},{op.BankAccount.Id},{op.Amount},{op.Date:yyyy-MM-dd},{op.Description},{op.Category.Id}");
        }

        return sb.ToString();
    }
}