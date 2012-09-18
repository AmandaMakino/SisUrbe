using System.Collections;

namespace Common.Logging.Context
{
    public class HttpContextLoggerInfo
    {
        public HttpContextLoggerInfo()
        {
            Items = new Hashtable();
        }

        public IDictionary Items { get; set; }

        public string SessionID { get; set; }

        public string UserHostAddress { get; set; }

        public string UserAgent { get; set; }

        public string UserName { get; set; }

        public string AbsoluteUri { get; set; }
    }
}
