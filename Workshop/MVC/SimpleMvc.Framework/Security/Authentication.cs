namespace SimpleMvc.Framework.Security
{
    public class Authentication
    {
        internal Authentication()
        {
            this.IsAuthenticated = false;
        }

        public Authentication(string name)
        {
            this.IsAuthenticated = true;
            this.Name = name;
        }

        public bool IsAuthenticated { get; }

        public string Name { get; }
    }
}
