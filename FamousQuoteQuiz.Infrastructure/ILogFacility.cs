namespace FamousQuoteQuiz.Infrastructure
{
    public interface ILogFacility<TSource>
    {
        void Debug(string message);

        void Error(string message);

        void Error(System.Exception ex);

        void Info(string message);

        void Trace(string message);

        void Warn(string message);
    }
}
