using System;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class RangeSisAttribute : ValidationSisAttribute
    {
        private readonly object minValue;
        private readonly object maxValue;
        private readonly Type objectType;

        public RangeSisAttribute(int minValue, int maxValue, string errorMessage)
        : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.objectType = typeof(int);
        }

        public RangeSisAttribute(double minValue, double maxValue, string errorMessage)
            : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.objectType = typeof(double);
        }

        public RangeSisAttribute(Type type, string minValue, string maxValue, string errorMessage)
            : base(errorMessage)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.objectType = type;
        }

        public override bool IsValid(object value)
        {
            if (this.objectType == typeof(int))
            {
                return (int) value >= (int) this.minValue && (int) value <= (int) this.maxValue;
            }

            if (this.objectType == typeof(double))
            {
                return (double)value >= (double) this.minValue && (double)value <= (double) this.maxValue;
            }

            if (this.objectType == typeof(decimal))
            {
                return (decimal)value >= decimal.Parse((string) this.minValue) && (decimal)value <= decimal.Parse((string) this.maxValue);
            }

            return false;
        }
    }
}
