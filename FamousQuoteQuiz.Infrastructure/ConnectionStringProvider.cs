namespace FamousQuoteQuiz.Infrastructure
{
    public class ConnectionStringProvider
    {
        public ConnectionStringProvider()
        {
            ConnectionString = "Server=DESKTOP-2OONPBE\\SQLEXPRESS;Database=FamousQuoteQuiz;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public string ConnectionString { get; private set; }
    }
}
