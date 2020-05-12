namespace _02._SystemTypeMethods
{
    using System;

    public class Person : IPerson
    {
        private int _age;

        public Person(int age, string name)
        {
            this.Age = age;
            this.Name = name;
        }

        protected Person()
        {
        }

        public string Name { get; private set; }

        public int Age
        {
            get => this._age;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Age value, cannot be below zero");
                }

                this._age = value;
            }
        }

        protected internal string MyProtectedInternalProperty { get; set; }

        public string WhoAmI()
        {
            return this.Name + " SomeRandomText";
        }

        [Obsolete]
        public void ObsoleteMethod()
        {
        }

        private static void PrivateStaticMethod()
        {
        }
    }
}
