using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Common.Logs
{
    /// <summary>
    /// default logging by diagnostic trace
    /// </summary>
    public class DefaultLoggingService : INoisLoggingService
    {
        public void LogError(string error, Exception ex)
        {
            Trace.TraceError(error + ex.InnerException);
        }

        public void LogInfo(string error, Exception ex)
        {
            Trace.TraceInformation(error);
        }

        public void LogFatal(string error, Exception ex)
        {
            Trace.TraceError(error);
        }

        public void LogWarning(string error, Exception ex)
        {
            Trace.TraceInformation(error);
        }

        public void LogDebug(string error, Exception ex)
        {
            Trace.TraceInformation(error);
        }

        public Task LogErrorAsync(string error, Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task LogInfoAsync(string error, Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task LogFatalAsync(string error, Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task LogWarningAsync(string error, Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task LogDebugAsync(string error, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
