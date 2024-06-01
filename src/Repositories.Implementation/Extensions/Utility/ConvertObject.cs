namespace Repositories.Implementation.Extensions.Utility
{
    public static class ConvertObject
    {
        public static string ToString<T>(T item)
        {
            var str = "";
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                {
                    var value = property.GetValue(item);
                    if (value == null)
                        continue;
                    str += value;
                }
            }
            return str;
        }
    }
}
