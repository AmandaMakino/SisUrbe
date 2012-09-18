namespace Common.Logging.Context
{
    public class EventType
    {
        public static readonly EventType LOGIN =                 new EventType("LI", "Login");
        public static readonly EventType LOGOUT =                new EventType("LO", "Logout");
        public static readonly EventType AUTH_TIMEOUT =          new EventType("TO", "Authentication Timeout");
        public static readonly EventType SYSTEM_CREATION =       new EventType("SC", "System Creation");
        public static readonly EventType SYSTEM_UPDATE =         new EventType("SU", "System Update");
        public static readonly EventType ACCOUNT_CREATION =      new EventType("AC", "Account Creation");
        public static readonly EventType ACCOUNT_UPDATE =        new EventType("AU", "Account Update");
        public static readonly EventType PASSWORD_CHANGE =       new EventType("PC", "Password Change");
        public static readonly EventType ADMIN_PASSWORD_CHANGE = new EventType("AP", "Admin Password Change");
        public static readonly EventType PASSWORD_RESET =        new EventType("PR", "Password Reset");
        public static readonly EventType ADMIN_PASSWORD_RESET =  new EventType("AR", "Admin Password Reset");
        public static readonly EventType USER_CREATION =         new EventType("UC", "User Creation");
        public static readonly EventType USER_UPDATE =           new EventType("UU", "User Update");
        public static readonly EventType USER_PROVISIONING =     new EventType("UP", "User Provisioning");
        public static readonly EventType USER_MAPPING =          new EventType("UM", "User Mapping");
        public static readonly EventType USER_GRANDFATHERING =   new EventType("UG", "User Grandfathering");

        public string Key { get; set; }
        public string Desc { get; set; }

        private EventType(string key, string description)
        {
            Key = key;
            Desc = description;
        }
    }
}
