namespace SimpleMvc.Framework.Attributes.Validation
{
    using System.Text.RegularExpressions;

    public class RegexAttribute : PropertyValidationAttribute
    {
        private readonly string _pattern;

        public RegexAttribute(string pattern)
        {
            this._pattern = $"^{pattern}$";
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            if (valueAsString == null)
            {
                return true;
            }

            return Regex.IsMatch(valueAsString, this._pattern);
        }
    }
}
