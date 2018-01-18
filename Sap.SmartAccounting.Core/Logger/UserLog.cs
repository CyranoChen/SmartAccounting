using System;
using System.Data;

namespace Sap.SmartAccounting.Core.Logger
{
    public class UserLog : Log, ILog
    {
        public void Debug(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Debug, message, string.Empty,
                para?.UserClient, trans);
        }

        public void Debug(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Debug, ex.Message, ex.StackTrace,
                para?.UserClient, trans);
        }

        public void Info(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Info, message, string.Empty,
                para?.UserClient, trans);
        }

        public void Info(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Info, ex.Message, ex.StackTrace,
                para?.UserClient, trans);
        }

        public void Warn(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Warn, message, string.Empty,
                para?.UserClient, trans);
        }

        public void Warn(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Warn, ex.Message, ex.StackTrace,
                para?.UserClient, trans);
        }

        public void Error(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Error, message, string.Empty,
                para?.UserClient, trans);
        }

        public void Error(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Error, ex.Message, ex.StackTrace,
                para?.UserClient, trans);
        }

        public void Fatal(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Fatal, message, string.Empty,
                para?.UserClient, trans);
        }

        public void Fatal(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Logging(GetType().Name, DateTime.Now, LogLevel.Fatal, ex.Message, ex.StackTrace,
                para?.UserClient, trans);
        }
    }
}