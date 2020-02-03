using FamousQuoteQuiz.Infrastructure;
using System.Security.Cryptography;

namespace BLL.Authentication
{
    public class LogInService
    {
        private readonly IHashingFacility hashing = null;

        public LogInService(IHashingFacility hash)
        {
            hashing = hash;
        }

        public bool Autenticate(string username, string password)
        {
            var result = false;
            var usernameCheck = false;
            var passwordCheck = false;

            using (MD5 md5Hash = MD5.Create())
            {
                var usernameHash = hashing.GetMd5Hash(md5Hash, username);
                usernameCheck= hashing.VerifyMd5Hash(md5Hash, username, usernameHash);
                var passwordHash = hashing.GetMd5Hash(md5Hash, password);
                passwordCheck = hashing.VerifyMd5Hash(md5Hash, password, passwordHash);
            }

            if (usernameCheck && passwordCheck)
            {
                result = true;
            }

            return result;
        }
    }
}
