using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GildedRose.Web.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResponseStatus
    {
        Success,
        Failure
    }

    public class ResponseModel
    {
        public ResponseStatus Status { get; set; }

        /// <summary>
        /// Optional message to return
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Optional payload to return
        /// </summary>
        public object Result { get; set; }
    }
}
