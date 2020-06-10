namespace GameStore.App.Infrastructure
{
    public static class StringExtensions
    {
        public static string Shortify(this string input, int length)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return input.Length > length 
                ? $"{input.Substring(0, length)}..." 
                : input;
        }
    }
}
