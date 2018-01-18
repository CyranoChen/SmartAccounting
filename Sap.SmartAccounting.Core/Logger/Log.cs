using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading;
using Sap.SmartAccounting.Core.Dapper;
using Sap.SmartAccounting.Core.Extension;

namespace Sap.SmartAccounting.Core.Logger
{
    [DbSchema("Log", Sort = "ID DESC")]
    public class Log : Entity<int>
    {
        protected void Logging(string logger, DateTime createTime, LogLevel level, string message,
            string stackTrace, UserClientInfo userClient = null, IDbTransaction trans = null)
        {
            var sql =
                @"INSERT INTO {0} (Logger, CreateTime, LogLevel, Message, IsException, StackTrace, Thread, Method, UserID, UserIP, UserBrowser, UserOS) 
                               VALUES (@logger, @createTime, @logLevel, @message, @isException, @stackTrace, @thread, @method, @userID, @userIP, @userBrowser, @userOS)";

            sql = string.Format(sql, Repository.GetTableAttr<Log>().Name);

            SqlParameter[] para =
            {
                new SqlParameter("@logger", logger),
                new SqlParameter("@createTime", createTime),
                new SqlParameter("@logLevel", level.ToString()),
                new SqlParameter("@message", message),
                new SqlParameter("@isException", !string.IsNullOrEmpty(stackTrace)),
                new SqlParameter("@stackTrace", stackTrace),
                new SqlParameter("@thread", string.Empty),
                new SqlParameter("@method", string.Empty),
                new SqlParameter("@userID", userClient?.UserID ?? -1),
                new SqlParameter("@userIP", userClient != null ? userClient.UserIP : "127.0.0.1"),
                new SqlParameter("@userBrowser", userClient != null ? userClient.UserBrowser : string.Empty),
                new SqlParameter("@userOS", userClient != null ? userClient.UserOS : string.Empty)
            };

            // no logging method
            var dapper = DapperHelper.GetInstance();
            dapper.Execute(sql, para.ToDapperParameters(), null, ignoreLog: true);
        }

        protected void Logging(string logger, DateTime createTime, LogLevel level, string message,
            string stackTrace, Thread thread, MethodBase method, UserClientInfo userClient = null, IDbTransaction trans = null)
        {
            var sql =
                @"INSERT INTO {0} (Logger, CreateTime, LogLevel, Message, IsException, StackTrace, Thread, Method, UserID, UserIP, UserBrowser, UserOS) 
                               VALUES (@logger, @createTime, @logLevel, @message, @isException, @stackTrace, @thread, @method, @userID, @userIP, @userBrowser, @userOS)";

            sql = string.Format(sql, Repository.GetTableAttr<Log>().Name);

            SqlParameter[] para =
            {
                new SqlParameter("@logger", logger),
                new SqlParameter("@createTime", createTime),
                new SqlParameter("@logLevel", level.ToString()),
                new SqlParameter("@message", message),
                new SqlParameter("@isException", !string.IsNullOrEmpty(stackTrace)),
                new SqlParameter("@stackTrace", stackTrace),
                new SqlParameter("@thread", thread.Name ?? thread.ManagedThreadId.ToString()),
                new SqlParameter("@method", method != null
                    ? $"{method.Name}, {method.DeclaringType?.FullName}"
                    : string.Empty),
                new SqlParameter("@userID", userClient?.UserID ?? -1),
                new SqlParameter("@userIP", userClient != null ? userClient.UserIP : "127.0.0.1"),
                new SqlParameter("@userBrowser", userClient != null ? userClient.UserBrowser : string.Empty),
                new SqlParameter("@userOS", userClient != null ? userClient.UserOS : string.Empty)
            };

            // no logging method
            var dapper = DapperHelper.GetInstance();
            dapper.Execute(sql, para.ToDapperParameters(), null, ignoreLog: true);
        }

        public static void Clean()
        {
            IRepository repo = new Repository();

            var list = repo.Query<Log>(x => x.Logger == "DaoLog" && x.CreateTime < DateTime.Now.AddMonths(-1))
                .FindAll(x => x.Level.Equals(LogLevel.Debug));

            if (list.Count > 0)
            {
                list.Delete();
            }
        }

        #region Members and Properties

        //[DbColumn("ID", IsKey = true)]
        //public int ID
        //{ get; set; }

        [DbColumn("Logger")]
        public string Logger { get; set; }

        [DbColumn("CreateTime")]
        public DateTime CreateTime { get; set; }

        [DbColumn("LogLevel")]
        public LogLevel Level { get; set; }

        [DbColumn("Message")]
        public string Message { get; set; }

        [DbColumn("IsException")]
        public bool IsException { get; set; }

        [DbColumn("StackTrace")]
        public string StackTrace { get; set; }

        [DbColumn("Thread")]
        public string Thread { get; set; }

        [DbColumn("Method")]
        public string Method { get; set; }

        [DbColumn("UserID")]
        public int UserID { get; set; }

        [DbColumn("UserIP")]
        public string UserIP { get; set; }

        [DbColumn("UserBrowser")]
        public string UserBrowser { get; set; }

        [DbColumn("UserOS")]
        public string UserOS { get; set; }

        #endregion
    }

    public class LogInfo
    {
        #region Members and Properties

        public Thread ThreadInstance { get; set; }

        public MethodBase MethodInstance { get; set; }

        public UserClientInfo UserClient { get; set; }

        #endregion
    }

    public class UserClientInfo
    {
        #region Members and Properties

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string UserIP { get; set; }

        public string UserBrowser { get; set; }

        public string UserOS { get; set; }

        #endregion
    }

    //FATAL（致命错误）：记录系统中出现的能使用系统完全失去功能，服务停止，系统崩溃等使系统无法继续运行下去的错误。例如，数据库无法连接，系统出现死循环。
    //ERROR（一般错误）：记录系统中出现的导致系统不稳定，部分功能出现混乱或部分功能失效一类的错误。例如，数据字段为空，数据操作不可完成，操作出现异常等。
    //WARN（警告）：记录系统中不影响系统继续运行，但不符合系统运行正常条件，有可能引起系统错误的信息。例如，记录内容为空，数据内容不正确等。
    //INFO（一般信息）：记录系统运行中应该让用户知道的基本信息。例如，服务开始运行，功能已经开户等。
    //DEBUG （调试信息）：记录系统用于调试的一切信息，内容或者是一些关键数据内容的输出。
    /// <summary>
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}