using System.Text.Json;
using CW_1_Sirotinkina_AD_234.Interfaces;
using CW_1_Sirotinkina_AD_234.Visitor;

namespace CW_1_Sirotinkina_AD_234.DataExport;

public class JSONDataExporter : DataExporter
{
    protected override IVisitor CreateVisitor()
    {
        return new UnifiedExportVisitor();
    }

    protected override string FormatData(IVisitor visitor)
    {
        var jsonVisitor = visitor as UnifiedExportVisitor;
        if (jsonVisitor == null)
            throw new InvalidOperationException("Неверный тип посетителя");

        var exportData = new
        {
            BankAccounts = jsonVisitor.Accounts,
            Categories = jsonVisitor.Categories,
            Operations = jsonVisitor.Operations
        };
        
        return JsonSerializer.Serialize(exportData, new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
        });
    }
}