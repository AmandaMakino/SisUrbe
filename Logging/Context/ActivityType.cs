namespace Common.Logging.Context
{
    public class ActivityType
    {
        public static readonly ActivityType LOGIN =                 new ActivityType("LI", "Login");
        public static readonly ActivityType LOGOUT =                new ActivityType("LO", "Logout");

        public string Key { get; set; }
        public string Desc { get; set; }

        private ActivityType(string key, string description)
        {
            Key = key;
            Desc = description;
        }
    }
}
