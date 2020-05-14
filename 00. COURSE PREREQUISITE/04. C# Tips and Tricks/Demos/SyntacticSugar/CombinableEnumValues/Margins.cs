namespace CombinableEnumValues
{
    using System;

    [Flags]
    public enum Margins
    {
        None = 0,
        Top = 1, //1,
        Left = 1 << 1, //2,
        Bottom = 1 << 2, //4,
        Right = 1 << 3,//8,
    }
}