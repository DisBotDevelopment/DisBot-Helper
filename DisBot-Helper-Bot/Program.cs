namespace DisBot_Helper_Bot;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await Startup.Start(args);
    }
}