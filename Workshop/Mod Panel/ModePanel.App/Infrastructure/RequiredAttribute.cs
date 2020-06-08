namespace ModePanel.App.Infrastructure
{
    using SimpleMvc.Framework.Attributes.Validation;

    public class RequiredAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
            => new System
                .ComponentModel
                .DataAnnotations
                .RequiredAttribute()
                .IsValid(value);
    }
}
