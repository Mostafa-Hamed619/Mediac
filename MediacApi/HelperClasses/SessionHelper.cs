using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace MediacApi.HelperClasses
{
    public static class SessionHelper
    {
        public static void SetObjectJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
