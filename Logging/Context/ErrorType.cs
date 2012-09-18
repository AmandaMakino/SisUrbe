namespace Common.Logging.Context
{
    public class ErrorType
    {
        public static readonly ErrorType C_SHARP =    new ErrorType("CS", "C#");
        public static readonly ErrorType JAVASCRIPT = new ErrorType("JS", "JavaScript");

        public string Key { get; set; }
        public string Desc { get; set; }

        private ErrorType(string key, string description)
        {
            Key = key;
            Desc = description;
        }
    }
}
