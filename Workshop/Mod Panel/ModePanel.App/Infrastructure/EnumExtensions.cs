namespace ModePanel.App.Infrastructure
{
    using System;
    using Data.Models;

    public static class EnumExtensions
    {
        public static string ToFriendlyName(this PositionType position)
        {
            return position switch
            {
                PositionType.Developer => position.ToString(),
                PositionType.Designer => position.ToString(),
                PositionType.Hr => position.ToString(),
                PositionType.TechnicalSupport => "Technical Support",
                PositionType.TechnicalTrainer => "Technical Trainer",
                PositionType.MarketingSpecialist => "Marketing Specialist",
                _ => throw new InvalidOperationException($"Invalid {nameof(PositionType)} type {position}")
            };
        }

        public static string ToViewClassName(this LogType type)
        {
            return type switch
            {
                LogType.CreatePost => "success",
                LogType.EditPost => "warning",
                LogType.DeletePost => "danger",
                LogType.UserApproval => "success",
                LogType.OpenMenu => "primary",
                _ => throw new InvalidOperationException($"Invalid {nameof(LogType)} type {type}")
            };
        }
    }
}
