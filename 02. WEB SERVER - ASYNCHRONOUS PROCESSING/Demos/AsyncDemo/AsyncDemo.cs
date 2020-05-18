namespace AsyncDemo
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class AsyncDemo
    {
        public static async Task Main()
        {
            await Test();
        }

        private static async Task Test()
        {
            try
            {
                var httpClient = new HttpClient();

                var httpResponse = await httpClient
                    .GetAsync("https://softuni2.bg");

                var result = await httpResponse.Content.ReadAsStringAsync();

                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Text");
        }
    }
}
