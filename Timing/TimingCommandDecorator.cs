using System.Diagnostics;

namespace CW_1_Sirotinkina_AD_234.Timing;

public class TimingCommandDecorator : ICommand
{
    private readonly ICommand _command;

    public TimingCommandDecorator(ICommand command)
    {
        _command = command;
    }

    public void Execute()
    {
        Stopwatch sw = Stopwatch.StartNew();
        _command.Execute();
        sw.Stop();
        Console.WriteLine($"Создание аккаунта заняло: {sw.ElapsedMilliseconds} ms.");
    }
}