using System.Text.Json.Serialization;

namespace EasyTransfer.Api.Dtos
{
    public class BankTransferDto
    {
        public string recipientsAccountNumber { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
