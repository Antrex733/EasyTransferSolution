using System.Text.Json.Serialization;

namespace EasyTransfer.Api.Dtos
{
    public class BankAccountDto
    {
        public string Name { get; set; }
        public decimal Balance { get; set; } = 0;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency Currency { get; set; } = Currency.PLN;
    }
}
