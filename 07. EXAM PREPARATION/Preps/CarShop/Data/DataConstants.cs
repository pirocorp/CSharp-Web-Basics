namespace CarShop.Data
{
    using System;

    public static class DataConstants
    {
        public const int DefaultMaxLength = 20;

        public const int DefaultMinLength = 4;

        public const int IdMaxLength = 40;

        public const int PlateMaxLength = 8;

        public const string Client = nameof(Client);

        public const string Mechanic = nameof(Mechanic);

        public const string CarPlateNumberRegularExpression = @"[A-Z]{1,2}[0-9]{4}[A-Z]{2}";

        public const int CarYearMinValue = 1900;

        public const int CarYearMaxValueOffset = 10;
    }
}
