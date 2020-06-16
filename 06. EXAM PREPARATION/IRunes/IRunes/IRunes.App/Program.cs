namespace IRunes.App
{
    using SIS.MvcFramework;

    public class Program
    {
        public async void Main()
        {
            await WebHost.StartAsync(new StartUp());
        }
    }
}
