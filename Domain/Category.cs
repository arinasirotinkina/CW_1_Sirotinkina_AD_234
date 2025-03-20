using CW_1_Sirotinkina_AD_234.Interfaces;

namespace CW_1_Sirotinkina_AD_234.Domain;

public class Category : IVisitable
{
    public enum CategoryType { Income, Expense };
    
    public Guid Id { get; set; }
    public CategoryType Type { get; set; }
    public string Name { get; set; }

    public Category(CategoryType type, string name)
    {
        Id = Guid.NewGuid();
        Type = type;
        Name = name;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}