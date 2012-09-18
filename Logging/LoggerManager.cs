using System;
using System.Collections.Generic;
using Common.Logging.Context;

namespace Common.Logging
{
    public class LoggerManager
    {
        private static Dictionary<string, Logger> _Loggers;
        private static Object LOGGER_DICTIONARY_LOCK = new Object();
        public static string ConnectionString { get; set; }

        public static ILogger GetLogger(Application app)
        {
            return GetLoggerInternal(app);
        }

        private static ILogger GetLoggerInternal(Application app)
        {
            lock (LOGGER_DICTIONARY_LOCK)
            {

                if (_Loggers == null)
                {
                    _Loggers = new Dictionary<string, Logger>();
                }

                if (!_Loggers.ContainsKey(app.Key))
                {
                    _Loggers[app.Key] = new Logger(app) {ConnectionString = ConnectionString};
                }
                return _Loggers[app.Key];
            }
        }
    }
}
