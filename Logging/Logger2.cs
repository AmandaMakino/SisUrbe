using System;
using System.Web;
using Common.Logging.Context;
using Common.Logging.DataLayer;

namespace Common.Logging
{
    public class Logger2 : ILogger2
    {
        private readonly LoggerDao2 _LoggerDao;

        #region Constructors

        public Logger2(Application app)
        {
            _LoggerDao = new LoggerDao2
            {
                App = app
            };
        }

        #endregion

        #region ILogger2 Methods

        public int OpenSession()
        {
            if (HttpContext.Current.Session[Keys.SESSION_ID_KEY] == null)
            {
                var sessionKey = HttpContext.Current.Session.SessionID;
                var ip = HttpContext.Current.Request.UserHostAddress;
                var userId = HttpContext.Current.User.Identity.Name;
                var userAgent = HttpContext.Current.Request.UserAgent;

                var sessionId = _LoggerDao.OpenSession(sessionKey, ip, userId, userAgent);
                HttpContext.Current.Session[Keys.SESSION_ID_KEY] = sessionId;
                return sessionId;
            }
            return (int)HttpContext.Current.Session[Keys.SESSION_ID_KEY];
        }

        // TODO: support for logging parameters
        public int OpenRequest(bool logParameters)
        {
            if (HttpContext.Current.Items[Keys.REQUEST_ID_KEY] == null)
            {
                var sessionKey = HttpContext.Current.Session.SessionID;
                var uri = HttpContext.Current.Request.Url.AbsoluteUri;

                var requestId = _LoggerDao.OpenRequest(sessionKey, uri);
                HttpContext.Current.Items[Keys.REQUEST_ID_KEY] = requestId;
                return requestId;
            }
            return (int)HttpContext.Current.Items[Keys.REQUEST_ID_KEY];
        }

        public int LogSecurityEvent(EventType type, Result result, string description)
        {
            var requestId = HttpContext.Current.Items[Keys.REQUEST_ID_KEY] as int?;
            return _LoggerDao.LogSecurityEvent(requestId, type, result, description);
        }

        public int LogError(string message, ErrorType type, Exception e)
        {
            var requestId = HttpContext.Current.Items[Keys.REQUEST_ID_KEY] as int?;
            var errorId = _LoggerDao.LogError(requestId, message, type, e);

            return errorId;
        }

        public int LogActivity(ActivityType type, string description)
        {
            var requestId = HttpContext.Current.Items[Keys.REQUEST_ID_KEY] as int?;
            return _LoggerDao.LogActivity(requestId, type, description);
        }

        public void CloseRequest(RequestStatus status)
        {
            var requestId = HttpContext.Current.Items[Keys.REQUEST_ID_KEY] as int?;
            _LoggerDao.CloseRequest(requestId, status);
        }

        public void CloseSession(SessionStatus status)
        {
            var sessionKey = HttpContext.Current.Session.SessionID;
            _LoggerDao.CloseSession(sessionKey, status);
        }

        #endregion

        #region Private Implementation

        public string ConnectionString
        {
            set
            {
                _LoggerDao.ConnectionString = value;
            }
        }
        
        #endregion

    }
}
