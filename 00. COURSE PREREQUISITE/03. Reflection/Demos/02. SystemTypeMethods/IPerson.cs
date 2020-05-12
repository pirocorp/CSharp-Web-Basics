namespace _02._SystemTypeMethods
{
    public interface IPerson
    {
        public int Age { get; }

        public string Name { get; }

        string WhoAmI();
    }
}
