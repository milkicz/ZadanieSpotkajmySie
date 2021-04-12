using System;
using System.Text.Json.Serialization;

namespace ZadanieSpotkajmySie
{
    public class TimeInterval
    {
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset start { get; set; }
        [JsonConverter(typeof(DateTimeOffsetConverter))]
        public DateTimeOffset end { get; set; }
    }
}
