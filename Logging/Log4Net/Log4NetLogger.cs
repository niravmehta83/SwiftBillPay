using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Logging.Log4Net
{
    public class Log4NetLogger : ILogger
    {

        private Log _logger;

        public Log4NetLogger(string file)
        {
            _logger = new Log(file);
        }

        public void Info(string message)
        {
            _logger.WriteLog(message);
        }

        public void Warn(string message)
        {
            _logger.WriteLog(message);
        }

        public void Debug(string message)
        {
            _logger.WriteLog(message);
        }

        public void Error(string message)
        {
            _logger.WriteLog(message);
        }

        public void Error(Exception x)
        {
            Error(LogUtility.BuildExceptionMessage(x));
        }

        public void Error(string message, Exception x)
        {
            _logger.WriteLog(message + LogUtility.BuildExceptionMessage(x));
        }

        public void Fatal(string message)
        {
            _logger.WriteLog(message);
        }

        public void Fatal(Exception x)
        {
            Fatal(LogUtility.BuildExceptionMessage(x));
        }
    }
}
