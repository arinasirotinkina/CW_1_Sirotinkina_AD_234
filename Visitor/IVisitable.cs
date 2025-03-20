namespace CW_1_Sirotinkina_AD_234.Interfaces;

public interface IVisitable
{ 
    void Accept(IVisitor visitor);
}