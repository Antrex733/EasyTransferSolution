
using EasyTransfer.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EasyTransfer.Api.Services
{
    public class AccountService: IAccountService
    {
        private readonly EasyTransferDBContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authentication;

        public AccountService(EasyTransferDBContext context, IMapper mapper,
            IPasswordHasher<User> hasher, AuthenticationSettings authentication)
        {
            _context = context;
            _mapper = mapper;
            _hasher = hasher;
            _authentication = authentication;
        }
        public bool RegisterUser(RegisterUserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);

            var hashedPassword = _hasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            return Save();
        }
        public string GenerateJwt(LoginUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(e => e.Email == dto.Email);
            if (user == null) 
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = 
                _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authentication.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_authentication.JwtExpireMinutes);

            var token = new JwtSecurityToken(_authentication.JwtIssuer,
                _authentication.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        public UserDto GetUser(int? userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
