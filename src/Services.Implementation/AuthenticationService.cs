using Contexts;
using Entities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.IApplicationServices;
using Services.Implementation.Extensions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Implementation
{
    public class AuthenticationService(HsmsDbContext context) : IAuthenticationService
    {
        private readonly HsmsDbContext _context = context;
        private readonly string alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        private readonly Random r = new();

        public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = _context.Teachers.FirstOrDefault(t => t.Id == userId)
                ?? throw new TeacherNotFoundException(userId);

            var currHash = HashPassword(user.Salt ??= "", oldPassword);
            if (currHash != user.Hash) throw new Exception("Mật khẩu không đúng");

            user.Hash = HashPassword(user.Salt ??= "", newPassword);

            await _context.SaveChangesAsync();
        }

        public async Task GenerateAccount(List<Guid> ids)
        {
            var users = ids.IsNullOrEmpty()
                ? _context.Teachers.ToList()
                : [.. _context.Teachers.Where(t => ids.Contains(t.Id))];
            Parallel.ForEach(users, user =>
            {
                user.Salt = GetSalt(5);
                user.Hash = HashPassword(salt: user.Salt, password: "Tinh12345@");
                user.Username = ConvertToUnsign($"{user.LastName}{user.Id.ToString()[..4]}".ToLower());
            });
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, string>> Login(string username, string password)
        {
            var user = await _context.Teachers.FirstOrDefaultAsync(u => u.Username == username)
                ?? throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng");

            if (user.Hash != HashPassword(user.Salt ??= "", password))
                throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng");

            var token = JwtUtils.GenerateToken(user);
            var name = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            var result = new Dictionary<string, string>() { { "user", name }, { "token", token } };

            return result;
        }

        public async Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task RenewToken()
        {
            throw new NotImplementedException();
        }

        public string GetSalt(int saltSize)
        {
            StringBuilder strB = new("");

            while ((saltSize--) > 0)
                strB.Append(alphanumeric[(int)(r.NextDouble() * alphanumeric.Length)]);
            return strB.ToString();
        }

        public static string HashPassword(string salt, string password)
        {
            string mergedPass = string.Concat(salt, password);
            return EncryptUsingMD5(mergedPass);
        }

        public static string EncryptUsingMD5(string inputStr)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5.HashData(Encoding.UTF8.GetBytes(inputStr));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static string RandomString(int size = 8)
        {
            StringBuilder builder = new();
            Random random = new();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
        public static string ConvertToUnsign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}
