using Entities.Common;
using Entities.DAOs;
using Entities.DTOs.TimetableCreation;
using Entities.RequestFeatures;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementation.Extensions
{
    public static partial class Utils
    {
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

        public static void ToCsv(this ETimetableFlag[,] timetableFlag, List<ClassTCDTO> classes)
        {
            var file = new StreamWriter("C:\\Users\\ponpy\\source\\repos\\KLTN\\10-be\\TimetableFlag.csv");
            var columnCount = timetableFlag.GetLength(0);
            var rowCount = timetableFlag.GetLength(1);
            file.Write("Tiết/Lớp,");
            foreach (var @class in classes)
            {
                file.Write("{0}", @class.Name);
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

        public static TimetableIndividual Clone(this TimetableIndividual src)
        {
            var classCount = src.TimetableFlag.GetLength(0);
            var periodCount = src.TimetableFlag.GetLength(1);
            var timetableFlag = new ETimetableFlag[classCount, periodCount];
            for (var i = 0; i < classCount; i++)
                for (var j = 0; j < periodCount; j++)
                    timetableFlag[i, j] = src.TimetableFlag[i, j];

            var timetableUnits = new List<TimetableUnitTCDTO>();
            foreach (var unit in src.TimetableUnits)
                timetableUnits.Add(unit with { });
            return new TimetableIndividual(timetableFlag, timetableUnits, src.Classes, src.Teachers);
        }

        public static void RandomlyAssign(this TimetableIndividual src)
        {
            for (var i = 0; i < src.TimetableFlag.GetLength(0); i++)
            {
                var startAts = new List<int>();
                for (var j = 1; j < src.TimetableFlag.GetLength(1); j++)
                    if (src.TimetableFlag[i, j] == ETimetableFlag.Unfilled)
                        startAts.Add(j);
                var timetableUnits = src.TimetableUnits
                    .Where(u => u.Assignment.Class.Id == src.Classes[i].Id && u.StartAt == 0)
                    .OrderBy(u => new Random().Next())
                    .ToList();
                if (startAts.Count != timetableUnits.Count) throw new Exception();
                for (var j = 0; j < timetableUnits.Count; j++)
                    timetableUnits[j].StartAt = startAts[j];
            }
        }

        public static List<TimetableIndividual> Crossover(
            this TimetableIndividual root, TimetableIndividual parent1, TimetableIndividual parent2, TimetableParameters parameters)
        {
            var rand = new Random();
            var child1 = root.Clone();
            var child2 = root.Clone();

            var classesTimetable1 = parent1.TimetableUnits.OrderBy(u => u.ClassName).ToList();
            var classesTimetable2 = parent2.TimetableUnits.OrderBy(u => u.ClassName).ToList();

            var randClassName = root.Classes[rand.Next(1, root.Classes.Count)].Name;
            var randIndex = 0;
            foreach (var unit in classesTimetable1)
                if (unit.ClassName.Equals(randClassName))
                {
                    randIndex = classesTimetable1.IndexOf(unit);
                    break;
                }

            for (var i = 0; i < root.TimetableUnits.Count; i++)
            {
                if (i < randIndex)
                {
                    child1.TimetableUnits.First(u => classesTimetable1[i].Id == u.Id).StartAt = classesTimetable1[i].StartAt;
                    child2.TimetableUnits.First(u => classesTimetable2[i].Id == u.Id).StartAt = classesTimetable2[i].StartAt;
                }
                else
                {
                    child1.TimetableUnits.First(u => classesTimetable2[i].Id == u.Id).StartAt = classesTimetable2[i].StartAt;
                    child2.TimetableUnits.First(u => classesTimetable1[i].Id == u.Id).StartAt = classesTimetable1[i].StartAt;
                }
            }
            var timetableUnits1 = child1.TimetableUnits.Where(u => u.ClassName == root.Classes[rand.Next(0, root.Classes.Count)].Name).ToList();
            var timetableUnits2 = child2.TimetableUnits.Where(u => u.ClassName == root.Classes[rand.Next(0, root.Classes.Count)].Name).ToList();

            Swap(timetableUnits1[rand.Next(0, timetableUnits2.Count / 2)], timetableUnits2[rand.Next(timetableUnits2.Count / 2, timetableUnits2.Count)]);
            Swap(timetableUnits2[rand.Next(0, timetableUnits2.Count / 2)], timetableUnits2[rand.Next(timetableUnits2.Count / 2, timetableUnits2.Count)]);

            child1.UpdateAdaptability(parameters);
            child2.UpdateAdaptability(parameters);

            return [child1, child2];
        }

        private static void Swap(TimetableUnitTCDTO a, TimetableUnitTCDTO b)
        {
            (a.StartAt, b.StartAt) = (b.StartAt, a.StartAt);
        }
    }
}
