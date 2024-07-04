using Contexts;
using Entities.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.IApplicationServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Implementation
{
    public class AuthenticationService(HsmsDbContext context) : IAuthenticationService
    {
        private readonly HsmsDbContext _context = context;

        public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = _context.Teachers.FirstOrDefault(t => t.Id == userId)
                ?? throw new TeacherNotFoundException(userId);

            var result = VerifyPassword(oldPassword, user.Hash ?? "", Encoding.ASCII.GetBytes(user.Salt ?? ""));
            if (!result) throw new Exception("Sai Mật Khẩu");

            var hash = HashPasword(newPassword, out var salt);

            user.Hash = hash;
            user.Salt = Convert.ToBase64String(salt);
            await _context.SaveChangesAsync();
        }

        public async Task GenerateAccount(List<Guid> ids)
        {
            var users = ids.IsNullOrEmpty()
                ? _context.Teachers.ToList()
                : _context.Teachers.Where(t => ids.Contains(t.Id)).ToList();
            Parallel.ForEach(users, user =>
            {
                var hash = HashPasword(RandomString(), out var salt);
                user.Hash = hash;
                user.Salt = Convert.ToBase64String(salt);
                user.Username = convertToUnsign($"{user.LastName}{user.Id.ToString()[..4]}".ToLower());
            });
            await _context.SaveChangesAsync();
        }

        public async Task Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task RenewToken()
        {
            throw new NotImplementedException();
        }

        const int keySize = 64;
        const int iterations = 350000;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        public static string convertToUnsign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        private string RandomString(int size = 8)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
