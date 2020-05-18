namespace TaskDemo
{
    using System;
    using System.Threading.Tasks;

    public static class TaskDemo
    {
        public static void Main()
        {
            //Tasks are like Promises in JS
            Task.Run(() =>
            {
                for (var i = 0; i < 1_000; i++)
                {
                    Console.WriteLine(i);
                }
            })
            .ContinueWith(previousTask =>
            {
                for (var i = 1_000; i < 2_000; i++)
                {
                    Console.WriteLine(i);
                }
            });

            while (true)
            {
                var line = Console.ReadLine();
                Console.WriteLine(line?.ToUpper());
            }
        }
    }
}
