namespace _04._MakeGenericType
{
    using System;
    using System.Collections.Generic;

    public static class Generics
    {
        public static void Main()
        {
            //Make Generic Type
            Type genericList = typeof(List<>);
            Type[] genericArguments = {typeof(string)};

            //result = typeof(List<string>);
            Type result = genericList.MakeGenericType(genericArguments);

            Console.WriteLine(result);

            //Make Generic Method
            var type = typeof(Generics);
            var myGenericMethodInfo = type.GetMethod("MyGenericMethod");
            var genericMethod = myGenericMethodInfo?.MakeGenericMethod(typeof(string));
            Console.WriteLine(genericMethod);
            genericMethod?.Invoke(null, null);

            //Instance of generic class
            type = typeof(DbSet<>);
            var genericType = type.MakeGenericType(new[] {typeof(string)});
            var instance = Activator.CreateInstance(genericType) as DbSet<string>;
            Console.WriteLine(instance.Count);

            //Invocation of Generic Method
            var personType = typeof(Person);
            var personInstance = Activator.CreateInstance(personType);
            personType
                .GetMethod("WhoAmI")?
                .MakeGenericMethod(typeof(string))
                .Invoke(personInstance, new object[] {string.Empty});
        }

        public static void MyGenericMethod<T>()
        {
            Console.WriteLine(typeof(T).Name);
        }
    }
}
