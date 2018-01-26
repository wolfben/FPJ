using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Utility.Helper
{
    public class LogHelper
    {
        static readonly ILog infoLog = LogManager.GetLogger("InfoLog");
        static readonly ILog errorLog = LogManager.GetLogger("ErrorLog");

        public static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Error(string msg)
        {
            errorLog.Error(msg);
        }

        public static void Error(string msg, Exception ex)
        {
            errorLog.Error(msg, ex);
        }

        public static void Error(string format, params object[] args)
        {
            errorLog.ErrorFormat(format, args);
        }

        public static void Info(string msg)
        {
            infoLog.Info(msg);
        }

        public static void Info(string msg, Exception ex)
        {
            infoLog.Info(msg, ex);
        }

        public static void Info(string format, params object[] args)
        {
            infoLog.InfoFormat(format, args);
        }
    }
}
