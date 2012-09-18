using System;
using Common.Logging.Context;

namespace Common.Logging
{
    public interface ILogger
    {
        void OpenSession();

        void OpenRequest(bool logParameters);

        void LogSecurityEvent(EventType type, Result result, string description);

        int LogError(string message, ErrorType type, Exception e);

        void LogActivity(ActivityType type, string description);

        void CloseRequest();

        void CloseSession();
    }
}
