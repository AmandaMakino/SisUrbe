namespace Common.Logging.Context
{
    public class Result
    {
        public static readonly Result SUCCESS =                 new Result("00", "Success");
        public static readonly Result UNKNOWN_USER =            new Result("UU", "Unknown User ID");
        public static readonly Result BAD_PASSWORD =            new Result("BP", "Bad Password");
        public static readonly Result USER_INACTIVE =           new Result("UI", "User Inactive");
        public static readonly Result ACCOUNT_WAS_BLOCKED =     new Result("AW", "Account was blocked");
        public static readonly Result ACCOUNT_IS_BLOCKED =      new Result("AI", "Account is blocked");
        public static readonly Result APP_AUTHORIZATION =       new Result("AA", "Application Not Authorized");
        public static readonly Result RESTRICTED_IP_RANGE =     new Result("IP", "Restricted IP Range");
        public static readonly Result REQUEST_SENT =            new Result("RS", "Request Sent");
        public static readonly Result MAPPING_FAIL =            new Result("MF", "Mapping failed");
        public static readonly Result MAPPING_SUCCESS =         new Result("MS", "Mapping succeeded");
        public static readonly Result UNEXPECTED_RESULT =       new Result("XX", "Unexpected result from stored procedure");
        public static readonly Result GRANDFATHERING_FAIL =     new Result("GF", "Grandfathering failed");
        public static readonly Result GRANDFATHERING_SUCCESS =  new Result("GS", "Grandfathering succeeded");
        public static readonly Result VALID_REQUEST =           new Result("VR", "Valid Request");
        public string Key { get; set; }
        public string Desc { get; set; }

        private Result(string key, string description)
        {
            Key = key;
            Desc = description;
        }

    }
}
