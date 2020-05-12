namespace _04._MakeGenericType
{
    using System;

    public class Person
    {
        public void WhoAmI<T>(T item)
        {
            Console.WriteLine(item.GetType());
        }
    }
}
