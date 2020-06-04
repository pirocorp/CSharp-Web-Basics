namespace SimpleMvc.Framework.Attributes.Validation
{
    public class NumberRangeAttribute : PropertyValidationAttribute
    {
        private readonly double _min;
        private readonly double _max;

        public NumberRangeAttribute(double min, double max)
        {
            this._min = min;
            this._max = max;

            this.ErrorMessage = $"Number is not in range [{this._min}, {this._max}]";
        }

        public override bool IsValid(object value)
        {
            var valueAsDouble = value as double?;

            if (valueAsDouble == null)
            {
                return true;
            }

            return this._min <= valueAsDouble && valueAsDouble <= this._max;
        }
    }
}
