using Serilog;
using System;

namespace FamousQuoteQuiz.Infrastructure
{
    public class SeriLogFacility<TSource> : ILogFacility<TSource>
    {
        private readonly ILogger Log = null;

        public SeriLogFacility(ILogger logFactory)
        {
            Log = logFactory.ForContext<TSource>();
        }

        public void Debug(string message) => Log.Debug(message);

        public void Error(string message) => Log.Error(message);

        public void Error(Exception ex) => Log.Error(ex, "Error occured");

        public void Info(string message) => Log.Information(message);

        public void Trace(string message) => Log.Verbose(message);

        public void Warn(string message) => Log.Warning(message);
    }
}
