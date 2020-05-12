namespace _03._InstancesAndInvocations
{
    public interface IPerson
    {
        public int Age { get; }

        public string Name { get; }

        string WhoAmI();
    }
}
