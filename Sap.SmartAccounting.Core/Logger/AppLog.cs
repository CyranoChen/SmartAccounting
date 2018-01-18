using System;
using System.Data;
using System.Diagnostics.Contracts;

namespace Sap.SmartAccounting.Core.Logger
{
    public class AppLog : Log, ILog
    {
        public void Debug(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Debug, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Debug, message, string.Empty, null, trans);
            }
        }

        public void Debug(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Debug, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Debug, ex.Message, ex.StackTrace, null, trans);
            }
        }

        public void Info(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Info, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Info, message, string.Empty, null, trans);
            }
        }

        public void Info(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Info, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Info, ex.Message, ex.StackTrace, null, trans);
            }
        }

        public void Warn(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Warn, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Warn, message, string.Empty, null, trans);
            }
        }

        public void Warn(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            Contract.Requires(ex != null);

            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Warn, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Warn, ex.Message, ex.StackTrace, null, trans);
            }
        }

        public void Error(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Error, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Error, message, string.Empty, null, trans);
            }
        }

        public void Error(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Error, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Error, ex.Message, ex.StackTrace, null, trans);
            }
        }

        public void Fatal(string message, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Fatal, message, string.Empty,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Fatal, message, string.Empty, null, trans);
            }
        }

        public void Fatal(Exception ex, LogInfo para = null, IDbTransaction trans = null)
        {
            if (para != null)
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Fatal, ex.Message, ex.StackTrace,
                    para.ThreadInstance, para.MethodInstance, null, trans);
            }
            else
            {
                Logging(GetType().Name, DateTime.Now, LogLevel.Fatal, ex.Message, ex.StackTrace, null, trans);
            }
        }
    }
}