using Bahar.Application.Dto.SigninVLogin;
using Bahar.Application.InterfaceRepository;
using Bahar.Common;
using Bahar.Domain;

public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator; 

    public AuthenticationService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = new PasswordHasher();
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public ResultDto<ResultRegisterUserDto> Register(RegisterUserRequestDto request)
    {
        if (_userRepository.Exists(request.Username, request.Email))
        {
            return new ResultDto<ResultRegisterUserDto>
            {
                IsSuccess = false,
                Message = "نام کاربری یا ایمیل قبلاً استفاده شده است."
            };
        }

        if (request.Password != request.RePassword)
        {
            return new ResultDto<ResultRegisterUserDto>
            {
                IsSuccess = false,
                Message = "رمز عبور و تکرار آن مطابقت ندارند."
            };
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            Address = "آدرس ثبت نشده",
            Mobile = "موبایل ثبت نشده",
            IsActive = true
        };

        _userRepository.Add(user);
        _userRepository.SaveChanges();

        return new ResultDto<ResultRegisterUserDto>
        {
            IsSuccess = true,
            Data = new ResultRegisterUserDto { UserId = user.Id }
        };
    }

    public ResultDto<LoginResponseDto> Login(LoginRequestDto request)
    {
        var user = _userRepository.GetByEmail(request.Email);
        if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            return new ResultDto<LoginResponseDto>
            {
                IsSuccess = false,
                Message = "ایمیل یا رمز عبور نادرست است."
            };
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new ResultDto<LoginResponseDto>
        {
            IsSuccess = true,
            Data = new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Token = token
            }
        };
    }
}
