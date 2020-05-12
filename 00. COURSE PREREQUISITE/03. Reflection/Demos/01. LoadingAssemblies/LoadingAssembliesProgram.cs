namespace _01._LoadingAssemblies
{
    using System;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Extension;

    public static class LoadingAssembliesProgram
    {
        public static void Main(string[] args)
        {
            // Example with Assembly.Load(name)
            Assembly assemblyLoad = Assembly.Load("01. LoadingAssemblies");

            // Example with Assembly.LoadFrom(path)
            Assembly assemblyLoadFrom = Assembly.LoadFrom("01. LoadingAssemblies.dll");

            // Difference between Calling and Executing Assembly
            Console.WriteLine($"LoadingAssemblies Executing Assembly: {Assembly.GetExecutingAssembly().FullName}");
            Console.WriteLine($"LoadingAssemblies Calling Assembly: {Assembly.GetCallingAssembly().FullName}");
            Console.WriteLine($"LoadingAssemblies Entry Assembly: {Assembly.GetEntryAssembly()?.FullName}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            var assemblyExtension = new Information().GetInfo();
            Console.WriteLine(assemblyExtension);
            Console.WriteLine(new string('-', Console.WindowWidth));

            // Already loaded assembly
            Console.WriteLine(typeof(object).Assembly.FullName);
            Console.WriteLine(typeof(Regex).Assembly.FullName);
            Console.WriteLine(Assembly.Load(new AssemblyName("netstandard")).FullName);
            Console.WriteLine(Assembly.Load(new AssemblyName("netstandard")).Location);
            Console.WriteLine(new string('-', Console.WindowWidth));
            
            Console.WriteLine("References:");
            foreach (var assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                Console.WriteLine(" -" + assembly.FullName);
            }

            Console.WriteLine(new string('-', Console.WindowWidth));

            Console.WriteLine("All .Net Standard references:");
            foreach (var assembly in Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies())
            {
                Console.WriteLine(" -" + assembly.FullName);
            }
        }
    }
}
