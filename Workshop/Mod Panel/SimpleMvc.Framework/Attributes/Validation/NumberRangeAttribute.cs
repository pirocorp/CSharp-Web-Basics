namespace SimpleMvc.Framework.Attributes.Validation
{
    public class NumberRangeAttribute : PropertyValidationAttribute
    {
        private readonly double _minimum;
        private readonly double _maximum;

        public NumberRangeAttribute(double minimum, double maximum)
        {
            this._minimum = minimum;
            this._maximum = maximum;
        }

        public override bool IsValid(object value)
        {
            var valueAsDouble = value as double?;
            if (valueAsDouble == null)
            {
                return true;
            }

            return this._minimum <= valueAsDouble && valueAsDouble <= this._maximum;
        }
    }
}
