using System;
using System.Collections.Generic;
using Common.Logging.Context;

namespace Common.Logging
{
    public class LoggerManager2
    {
        private static Dictionary<string, Logger2> _Loggers;
        private static readonly Object _LoggerDictionaryLock = new Object();
        public static string ConnectionString { get; set; }

        public static ILogger2 GetLogger(Application app)
        {
            return GetLoggerInternal(app);
        }

        private static ILogger2 GetLoggerInternal(Application app)
        {
            lock (_LoggerDictionaryLock)
            {
                if (_Loggers == null)
                {
                    _Loggers = new Dictionary<string, Logger2>();
                }

                if (!_Loggers.ContainsKey(app.Key))
                {
                    _Loggers[app.Key] = new Logger2(app) {
                        ConnectionString = ConnectionString
                    };
                }
                return _Loggers[app.Key];
            }
        }
    }
}
