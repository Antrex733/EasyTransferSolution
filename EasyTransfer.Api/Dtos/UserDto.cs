using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EasyTransfer.Api.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
