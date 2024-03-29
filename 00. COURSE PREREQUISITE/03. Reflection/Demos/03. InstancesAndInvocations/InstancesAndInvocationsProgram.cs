﻿namespace _03._InstancesAndInvocations
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class InstancesAndInvocationsProgram
    {
        public static void Main()
        {
            var personType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == "Person");

            //Create instance by calling the constructor method
            Type[] constructorArgs = {typeof(int), typeof(string)};
            ConstructorInfo ctor = personType?.GetConstructor(constructorArgs);
            object[] constructorParams = {22, "Viktor Dakov"};
            var instanceWithCtor = ctor?.Invoke(constructorParams) as IPerson;
            Console.WriteLine(instanceWithCtor);

            //Create instance with Activator.CreateInstance
            object[] myConstructorParams = {29, "Nikolay Kostov"};
            var instance = Activator.CreateInstance(personType, myConstructorParams) as Person;
            Console.WriteLine(instance);

            //Invoke Eat
            MethodInfo eatMethod = typeof(Person).GetMethod("Eat");
            object eatMethodResult = eatMethod.Invoke(instance, new object[] {"Burger"});
            Console.WriteLine(eatMethodResult);

            //Work with Get and Set
            var propertyName = personType.GetProperty("Name");
            object nameValue = propertyName.GetValue(instance);
            Console.WriteLine(nameValue);
            propertyName.SetValue(instance, "Ivaylo Kenov");
            nameValue = propertyName.GetValue(instance);
            Console.WriteLine(nameValue);
        }
    }
}
