namespace ModePanel.App.Infrastructure
{
    using System;
    using Data.Models;

    public static class EnumExtensions
    {
        public static string ToFriendlyName(this PositionType position)
        {
            switch (position)
            {
                case PositionType.Developer:
                case PositionType.Designer:
                case PositionType.HR:
                    return position.ToString();
                case PositionType.TechnicalSupport:
                    return "Technical Support";
                case PositionType.TechnicalTrainer:
                    return "Technical Trainer";
                case PositionType.MarketingSpecialist:
                    return "Marketing Specialist";
                default:
                    throw new ArgumentOutOfRangeException(nameof(position), position, null);
            }
        }
    }
}
