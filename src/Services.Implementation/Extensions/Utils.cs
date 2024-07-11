using Entities.Common;
using Entities.DAOs;
using Entities.DTOs.TimetableCreation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Services.Implementation.Extensions
{
    public static partial class Utils
    {
        private static readonly Random rand = new();

        public static string GenerateToken(Teacher teacher)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("MyAccessToken");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", teacher.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("MyAccessToken");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                return userId;
            }
            catch
            {
                return null;
            }
        }

        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var deletedItems = list1.Except(list2).Any();
            var newItems = list2.Except(list1).Any();
            return !newItems && !deletedItems;
        }

        public static void Swap(TimetableUnitTCDTO a, TimetableUnitTCDTO b)
        {
            (a.StartAt, b.StartAt) = (b.StartAt, a.StartAt);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                int swapIndex = rand.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        public static void ToCsv(this TimetableIndividual src)
        {
            var path = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\Timetable.csv";
            var errorPath = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\TimetableError.txt";

            //var path = "D:\\Workspace\\dotnet-asp\\fix\\10-be\\Timetable.csv";
            //var errorPath = "D:\\Workspace\\dotnet-asp\\fix\\10-be\\TimetableError.txt";

            var file = new StreamWriter(path);
            var columnCount = src.TimetableFlag.GetLength(0);
            var rowCount = src.TimetableFlag.GetLength(1);
            file.Write("Tiết,");
            for (var i = 0; i < src.Classes.Count; i++)
            {
                file.Write("{0}", src.Classes[i].Name);
                file.Write(",");
            }
            file.WriteLine();

            for (int row = 1; row < rowCount; row++)
            {
                file.Write("{0}", row);
                file.Write(",");
                for (int column = 0; column < columnCount; column++)
                {
                    var unit = src.TimetableUnits.FirstOrDefault(u => u.StartAt == row && u.ClassName == src.Classes[column].Name);
                    file.Write($"{unit?.SubjectName} - {unit?.TeacherName}");
                    file.Write(",");
                }
                file.WriteLine();
            }
            file.Close();

            file = new StreamWriter(errorPath);
            for (var i = 0; i < src.ConstraintErrors.Count; i++)
                if (src.ConstraintErrors[i].IsHardConstraint)
                    file.WriteLine("Lỗi: " + src.ConstraintErrors[i].Description);
                else
                    file.WriteLine("Lưu ý: " + src.ConstraintErrors[i].Description);
            file.Close();
        }

        public static void ToCsv(this ETimetableFlag[,] timetableFlag, List<ClassTCDTO> classes)
        {
            var path = "C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\TimetableFlag.csv";
            //var path = "D:\\Workspace\\dotnet-asp\\fix\\10-be\\TimetableFlag.csv";
            var file = new StreamWriter(path);
            var columnCount = timetableFlag.GetLength(0);
            var rowCount = timetableFlag.GetLength(1);
            file.Write("Tiết/Lớp,");
            for (var i = 0; i < classes.Count; i++)
            {
                file.Write("{0}", classes[i].Name);
                file.Write(",");
            }
            file.WriteLine();

            for (int row = 1; row < rowCount; row++)
            {
                file.Write("{0}", row);
                file.Write(",");
                for (int column = 0; column < columnCount; column++)
                {
                    file.Write("{0}", timetableFlag[column, row]);
                    file.Write(",");
                }
                file.WriteLine();
            }
            file.Close();
        }

        public static void JsonOutput(this object obj, string fileName = "JsonOutput")
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            var json = JsonSerializer.Serialize(obj, jso);
            //var file = new StreamWriter($"C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\{fileName}.json");
            var file = new StreamWriter($"D:\\Workspace\\dotnet-asp\\fix\\10-be\\{fileName}.json");
            file.Write(json);
            file.Close();
        }
    }
}
