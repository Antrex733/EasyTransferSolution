namespace EasyTransfer.Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<RegisterUserDto, LoginUserDto>();
        }
    }
}
