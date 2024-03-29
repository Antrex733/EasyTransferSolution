﻿namespace EasyTransfer.Api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<RegisterUserDto, LoginUserDto>();
            CreateMap<User, UserDto>();
            CreateMap<BankAccountDto, BankAccount>();
        }
    }
}
