namespace CW_1_Sirotinkina_AD_234.Observer;

public interface IBankAccountObserver
{
    void HandleBalanceChanged(object sender, EventArgs e);
}