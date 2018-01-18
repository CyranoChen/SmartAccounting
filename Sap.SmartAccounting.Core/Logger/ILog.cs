using System;
using System.Data;

namespace Sap.SmartAccounting.Core.Logger
{
    public interface ILog
    {
        void Debug(string message, LogInfo para = null, IDbTransaction trans = null);
        void Debug(Exception ex, LogInfo para = null, IDbTransaction trans = null);

        void Info(string message, LogInfo para = null, IDbTransaction trans = null);
        void Info(Exception ex, LogInfo para = null, IDbTransaction trans = null);

        void Warn(string message, LogInfo para = null, IDbTransaction trans = null);
        void Warn(Exception ex, LogInfo para = null, IDbTransaction trans = null);

        void Error(string message, LogInfo para = null, IDbTransaction trans = null);
        void Error(Exception ex, LogInfo para = null, IDbTransaction trans = null);

        void Fatal(string message, LogInfo para = null, IDbTransaction trans = null);
        void Fatal(Exception ex, LogInfo para = null, IDbTransaction trans = null);
    }
}