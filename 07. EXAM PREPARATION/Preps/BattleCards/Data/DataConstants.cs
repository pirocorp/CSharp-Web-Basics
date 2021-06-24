namespace BattleCards.Data
{
    public static class DataConstants
    {
        public const int IdMaxLength = 40;

        public const int UserMinUsername = 5;
        public const int UserMaxUsername = 20;

        public const int UserMinPassword = 6;
        public const int UserMaxPassword = 20;

        public const int CardMinName = 5;
        public const int CardMaxName = 15;

        public const int CardMaxDescription = 200;

        public static readonly string[] KeywordValues = {
            "Tough", 
            "Challenger", 
            "Elusive", 
            "Overwhelm", 
            "Lifesteal", 
            "Ephemeral", 
            "Fearsome"
        };

        public const string UserEmailRegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
    }
}
