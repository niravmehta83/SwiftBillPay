﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Logging.Log4Net;

namespace Logging
{
    public static class LogFactory
    {
        /// <summary>
        /// This static class returns the logger that is configured through the appSettings section of the web.config file
        /// The default logger is NLog
        /// </summary>
        /// <returns>A concrete logger, usually an NLogLogger or Log4NetLogger</returns>
        public static ILogger Logger(string file)
        {
            string defaultLoggerTypeName = "MvcLoggingDemo.Services.Logging.NLog.NLogLogger";
            string loggerTypeName = ConfigurationManager.AppSettings["loggerTypeName"];
            loggerTypeName = (loggerTypeName == null) ? defaultLoggerTypeName : loggerTypeName;

            Type loggerType = Type.GetType(loggerTypeName);
            //ILogger logger = Activator.CreateInstance(loggerType) as ILogger;
            ILogger logger = new Log4NetLogger(file);

            return logger;
        }
    }
}
