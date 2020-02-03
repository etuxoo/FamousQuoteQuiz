using System.Security.Cryptography;

namespace FamousQuoteQuiz.Infrastructure
{
    public interface IHashingFacility
    {
        public string GetMd5Hash(MD5 md5Hash, string input);

        public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash);
    }
}
