namespace EasyTransfer.Api.Models
{
    public class Blik
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime WillExpire { get; set; } = DateTime.Now.AddMinutes(20);
    }
}
