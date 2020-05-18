namespace Primes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    public static class PrimesProgram
    {
        private static readonly Random _rnd = new Random();

        public static void Main()
        {
            var lockObject = new object();
            var count = 0;

            // Declare a local function.
            // Local function can be used to create closure just like in JS
            void CountPrimeNumbers(int from, int to)
            {
                var isPrime = true;

                for (var number = from; number <= to; number++)
                {
                    for (var divisor = 2; divisor <= Math.Sqrt(number); divisor++)
                    {
                        if (number % divisor == 0)
                        {
                            isPrime = false;
                        }
                    }

                    if (isPrime)
                    {
                        lock (lockObject)
                        {
                            count++;
                        }
                    }

                    isPrime = true;
                }
            }

            var threads = new List<Thread>();

            for (var i = 0; i < 4; i++)
            {
                var id = i; // id is closure for thread object 
                //id saves current value of i for the time when thread need it  

                var thread = new Thread(() =>
                {
                    Console.WriteLine($"Thread {id} has started");
                    Thread.Sleep(_rnd.Next(0, 2000));
                    CountPrimeNumbers(1 + 500_000 * id, 500_000 + 500_000 * id);
                    Console.WriteLine($"Thread {id} has finished");
                });
                
                threads.Add(thread);
            }
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join()); //Join is blocking operation

            stopWatch.Stop();

            Console.WriteLine(count); 
            Console.WriteLine(stopWatch.Elapsed);
        }
    }
}
