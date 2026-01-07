namespace R2000Wpf;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        var app = new App();

        var builder = App.CreateBuilder();

        app = (App)builder.Build();

        app.RunWithStartingWindow<MainWindow>();
    }
}
