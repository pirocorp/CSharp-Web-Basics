namespace SimpleMvc.Framework.Attributes.Validation
{
    using System;

    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public abstract class PropertyValidationAttribute : Attribute
    {
        protected PropertyValidationAttribute()
        {
            this.ErrorMessage = "Invalid model";
        }

        public string ErrorMessage { get; protected set; }

        public abstract bool IsValid(object value);
    }
}
