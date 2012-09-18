namespace Common.Logging.Context
{
    public class RequestStatus
    {
        public static readonly RequestStatus OPEN   = new RequestStatus("O", "Request currently open");
        public static readonly RequestStatus NORMAL = new RequestStatus("N", "Request completed normally");
        public static readonly RequestStatus ERROR  = new RequestStatus("E", "Request completed with error");
        public static readonly RequestStatus CRASH  = new RequestStatus("X", "Request incomplete due to server crash");

        public string Key { get; set; }
        public string Desc { get; set; }

        private RequestStatus(string key, string description)
        {
            Key = key;
            Desc = description;
        }
    }
}
