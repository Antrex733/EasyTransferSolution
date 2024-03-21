namespace EasyTransfer.Api.Services.Interfaces
{
    public interface IUserService
    {
        public bool RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginUserDto dto);
        public UserDto GetUser(int? userId);
        bool Save();
    }
}
