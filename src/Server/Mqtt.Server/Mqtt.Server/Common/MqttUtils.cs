using System;
using System.Linq;
using Newtonsoft.Json;

namespace Mqtt.Server.Common
{
    internal static class MqttUtils
    {
        public static string ToString(string title, object data)
        {
            var json = JsonConvert.SerializeObject(
                data,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
            var strings = json
               .Replace("\"", "")
               .Split(Environment.NewLine)
               .Skip(1)
               .SkipLast(1)
               .ToList();
            strings.Insert(0, $" -  {title}:");
            strings.Add("");

            return string.Join(Environment.NewLine, strings);
        }
    }
}