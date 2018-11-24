namespace GildedRose.Api.Models
{
    public class ResponseError
    {
        public string Field { get; set; }

        public string ErrorMessage { get; set; }

        public object InputData { get; set; }
    }
}
