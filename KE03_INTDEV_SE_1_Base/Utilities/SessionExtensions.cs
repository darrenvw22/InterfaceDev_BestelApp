using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace KE03_INTDEV_SE_1_Base.Utilities
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var str = session.GetString(key);
            if (string.IsNullOrEmpty(str)) return default;
            return JsonSerializer.Deserialize<T>(str);
        }
    }
}
