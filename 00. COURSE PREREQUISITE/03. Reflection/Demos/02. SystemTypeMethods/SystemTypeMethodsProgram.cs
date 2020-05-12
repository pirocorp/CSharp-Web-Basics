namespace _02._SystemTypeMethods
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class SystemTypeMethodsProgram
    {
        public static void Main()
        {
            Type personType = typeof(Person);
            //Type personType = Type.GetType("_02._SystemTypeMethods.Person");

            //Type Info
            Console.WriteLine($"FullName: {personType.FullName}");
            Console.WriteLine($"Namespace: {personType.Namespace}"); 
            Console.WriteLine($"Name: {personType.Name}");
            Console.WriteLine($"Assembly.FullName: {personType.Assembly.FullName}"); 
            Console.WriteLine($"BaseType: {personType.BaseType}");
            Console.WriteLine($"Interfaces: {string.Join(", ", personType.GetInterfaces().Select(i => i.Name))}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            //Fields Info
            FieldInfo nullField = personType.GetField("_age"); //not found
            Console.WriteLine($"Searching for '_age': {nullField}");
            FieldInfo privateField = personType.GetField("_age", BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine($"Searching for non-public instance '_age': {privateField}");
            FieldInfo[] fields = personType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine($"All fields: {string.Join(", ", fields.Select(f => $"{f.FieldType.Name} {f.Name}").ToList())}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            //Method Info
            MethodInfo whoAmIMethod = personType.GetMethod("WhoAmI");
            Console.WriteLine($"Searching for 'WhoAmI': {whoAmIMethod}");
            MethodInfo obsoleteMethod = personType.GetMethod("ObsoleteMethod", BindingFlags.Instance);
            Console.WriteLine($"Searching for static 'ObsoleteMethod': {obsoleteMethod}");
            MethodInfo[] methods = personType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            Console.WriteLine($"All methods: \n{string.Join(",\n", methods.Select(x => $" -{x}").ToList())}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            //Property Info
            PropertyInfo nameProperty = personType.GetProperty("Name");
            Console.WriteLine($"Searching for 'Name': {nameProperty}");
            PropertyInfo protectedInternalProperty = personType.GetProperty("MyProtectedInternalProperty", BindingFlags.NonPublic | BindingFlags.Instance);
            Console.WriteLine($"Searching for 'MyProtectedInternalProperty': {protectedInternalProperty}");
            PropertyInfo[] properties = personType.GetProperties();
            Console.WriteLine($"All properties: {string.Join(", ", properties.ToList())}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            //Constructor Info
            ConstructorInfo constructorInfo = personType.GetConstructor(new[] {typeof(int), typeof(string)});
            Console.WriteLine($"Searching for .ctor(int, string): {constructorInfo}");
            ConstructorInfo protectedConstructor = personType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] {}, 
                null);
            Console.WriteLine($"Searching for non-public .ctor(): {protectedConstructor}");
            ConstructorInfo[] constructors = personType.GetConstructors();
            Console.WriteLine($"All public constructors: {string.Join(", ", constructors.ToList())}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            //Member Info
            MemberInfo[] membersInfo = personType
                .GetMembers(BindingFlags.NonPublic 
                             | BindingFlags.Public 
                             | BindingFlags.Instance 
                             | BindingFlags.Static);
            Console.WriteLine("All members");

            foreach (var memberInfo in membersInfo)
            {
                Console.WriteLine($"  -{memberInfo.MemberType}: {memberInfo}");
            }

            Console.WriteLine(new string('-', Console.WindowWidth));

            //Search members with given attribute
            var attributeInfo = personType
                .GetMembers(BindingFlags.NonPublic
                            | BindingFlags.Public
                            | BindingFlags.Instance
                            | BindingFlags.Static)
                .Where(x => x.GetCustomAttributes(typeof(ObsoleteAttribute)).Any());

            Console.WriteLine("Members with ObsoleteAttribute: " + string.Join(", ", attributeInfo.ToList()));
        }
    }
}
