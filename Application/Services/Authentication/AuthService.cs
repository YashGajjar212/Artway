using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Authentication;
using Artway.Application.Interfaces.Customers;
using Artway.Application.Interfaces.Token;
using Artway.Infrastructure.Models.Customers;
using Artway.Presentation.DTOs.Auth;
using Artway.Presentation.DTOs.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Artway.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        //private readonly IAuthRepository _authRepository;
        private readonly IAccountService _accountServices;

        private readonly IMapper _mapper;

        private readonly IPasswordHasher<Account> _passwordHasher;

        private readonly ITokenService _tokenService;

        public AuthService(IPasswordHasher<Account> passwordHasher,
            IAccountService accountServices, 
            IMapper mapper,
            ITokenService tokenService
            ) // IAuthRepository authRepository, 
        {
            //_authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _accountServices = accountServices;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public AuthTokenDto GetJWTToken()
        {
            var result = _tokenService.GenerateToken("dummyEmail");

            AuthTokenDto response = new AuthTokenDto();
            response.Issuer = "Artway_App";
            response.Token = result;
            response.TokenType = "jwt";
            response.ExpireyInSeconds = 20;
            response.ExpiresAt = DateTime.Now.AddMinutes(20);

            return response;
        }

        public async Task<RegisterResponseDto> RegisterAccount(RegisterRequestDto registerRequestDto)
        {
            var existingAccount = await _accountServices.GetAccountByEmail(registerRequestDto.Email);

            if (existingAccount != null)
                throw new Exception("Account already exists");

            Account newAccount = new Account();
            newAccount.Email = registerRequestDto.Email;
            newAccount.PasswordHash = _passwordHasher.HashPassword(newAccount, registerRequestDto.Password);

            var mapAccount = _mapper.Map<AccountDto>(newAccount);

            var result = await _accountServices.AddAccount(mapAccount);

            if (result == null)
                throw new Exception("Unable to add new account");

            var token = _tokenService.GenerateToken(newAccount.Email);

            RegisterResponseDto registerResponseDto = new RegisterResponseDto();
            registerResponseDto.AccountId = result.AccountId;
            registerResponseDto.Email = result.Email;
            registerResponseDto.Token = token;

            return registerResponseDto;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var account = await _accountServices.GetAccountByEmail(loginRequestDto.Email);

            if (account == null)
                throw new NotFoundException($"Account does not exist");

            var mapAccount = _mapper.Map<Account>(account);

            var result = _passwordHasher.VerifyHashedPassword(mapAccount, account.PasswordHash, loginRequestDto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException($"Invalid email or password.");

            var token = _tokenService.GenerateToken(loginRequestDto.Email);

            LoginResponseDto response = new LoginResponseDto();
            response.Email = loginRequestDto.Email;
            response.ExpiresAt = DateTime.UtcNow.AddMinutes(20);
            response.Token = token;

            return response;
        }
    }
}