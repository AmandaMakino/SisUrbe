namespace Common.Logging.Context
{
    public class SessionStatus
    {
        public static readonly SessionStatus OPEN             = new SessionStatus("O", "Session currently open");
        public static readonly SessionStatus LOGGED_OUT       = new SessionStatus("L", "Session logged out by user");
        public static readonly SessionStatus LOGGED_OUT_ADMIN = new SessionStatus("A", "Session logged out by administrator");
        public static readonly SessionStatus TIMEOUT          = new SessionStatus("T", "Session timed out");
        public static readonly SessionStatus CRASH            = new SessionStatus("X", "Session lost due to server crash");

        public string Key { get; set; }
        public string Desc { get; set; }

        private SessionStatus(string key, string description)
        {
            Key = key;
            Desc = description;
        }
    }
}
