using System;
using System.Web;
using Common.Logging.Context;
using Common.Logging.DataLayer;

namespace Common.Logging
{
    public class Logger : ILogger
    {
        private readonly LoggerDao _LoggerDao;

        #region Constructors

        public Logger(Application app)
        {
            _LoggerDao = new LoggerDao
            {
                App = app
            };
        }

        #endregion

        #region ILogger Methods

        public void OpenSession()
        {
            throw new NotImplementedException();
        }

        public void OpenRequest(bool logParameters)
        {
            throw new NotImplementedException();
        }

        public void LogSecurityEvent(EventType type, Result result, string description)
        {
            var wrapper = GetLoggerInfo();
            _LoggerDao.LogSecurityEventInternal(type, result, description, wrapper);
        }

        public int LogError(string message, ErrorType type, Exception e)
        {
            var wrapper = GetLoggerInfo();
            return _LoggerDao.LogErrorInternal(message, type, e, wrapper);
        }


        private static HttpContextLoggerInfo GetLoggerInfo()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            var sessionId = HttpContext.Current.Session == null ? null : HttpContext.Current.Session.SessionID;
            return new HttpContextLoggerInfo
            {
                Items = HttpContext.Current.Items,

                SessionID = sessionId,

                UserHostAddress = HttpContext.Current.Request.UserHostAddress,
                UserAgent = HttpContext.Current.Request.UserAgent,
                AbsoluteUri = HttpContext.Current.Request.Url.AbsoluteUri,

                UserName = HttpContext.Current.User.Identity.Name
            }; 
            
        }

        public void LogActivity(ActivityType type, string description)
        {
            throw new NotImplementedException();
        }

        public void CloseRequest()
        {
            throw new NotImplementedException();
        }

        public void CloseSession()
        {
            throw new NotImplementedException();
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
