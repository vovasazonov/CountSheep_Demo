using System;
using UnityEngine;

namespace Project.CoreDomain.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        public LoggerService()
        {
            OsyaLogger.SetLogger(this);
        }

        public void Log(string text)
        {
            Debug.Log(text);
        }

        public void LogError(string text)
        {
            Debug.LogError(text);
        }

        public void LogWarning(string text)
        {
            Debug.LogWarning(text);
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
