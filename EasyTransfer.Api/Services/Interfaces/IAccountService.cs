namespace EasyTransfer.Api.Services.Interfaces
{
    public interface IAccountService
    {
        public bool RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginUserDto dto);
        bool Save();
    }
}
