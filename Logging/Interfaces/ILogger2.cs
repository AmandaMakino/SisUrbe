using System;
using Common.Logging.Context;

namespace Common.Logging
{
    public interface ILogger2
    {
        int OpenSession();

        int OpenRequest(bool logParameters);

        int LogSecurityEvent(EventType type, Result result, string description);

        int LogError(string message, ErrorType type, Exception e);

        int LogActivity(ActivityType type, string description);

        void CloseRequest(RequestStatus status);

        void CloseSession(SessionStatus status);
    }
}
