using Artway.Application.Exceptions;
using Artway.Application.Interfaces.Authentication;
using Artway.Application.Interfaces.Customers;
using Artway.DTOs.Auth;
using Artway.DTOs.Customers;
using Artway.Models.Auth;
using Artway.Models.Customers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Artway.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        //private readonly IAuthRepository _authRepository;
        private readonly ICustomerServices _customerServices;

        private readonly IMapper _mapper;

        private readonly IPasswordHasher<Customer> _passwordHasher;

        public AuthService(IPasswordHasher<Customer> passwordHasher, ICustomerServices customerServices, IMapper mapper
            ) // IAuthRepository authRepository, 
        {
            //_authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _customerServices = customerServices;
            _mapper = mapper;
        }

        public async Task<RegisterResponseDto> RegisterCustomer(RegisterRequestDto registerRequestDto)
        {
            var existingCustomer = await _customerServices.GetCustomerByEmail(registerRequestDto.Email);

            if (existingCustomer != null)
                throw new Exception($"Customer already exists with email: {registerRequestDto.Email}");

            Customer newCustomer = new Customer();
            newCustomer.Email = registerRequestDto.Email;
            newCustomer.PasswordHash = _passwordHasher.HashPassword(newCustomer, registerRequestDto.Password);

            var mapCustomer = _mapper.Map<CustomerDto>(newCustomer);

            var result = await _customerServices.AddCustomer(mapCustomer);

            if (result == null)
                throw new Exception($"Unable to add new customer with email: {newCustomer.Email}");

            RegisterResponseDto registerResponseDto = new RegisterResponseDto();
            registerResponseDto.CustomerId = result.CustomerId;
            registerResponseDto.Email = result.Email;

            return registerResponseDto;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var customer = await _customerServices.GetCustomerByEmail(loginRequestDto.Email);

            if (customer == null)
                throw new NotFoundException($"Customer does not exist");

            var mapCustomer = _mapper.Map<Customer>(customer);

            var result = _passwordHasher.VerifyHashedPassword(mapCustomer, customer.PasswordHash, loginRequestDto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException($"Invalid email or password.");

            LoginResponseDto response = new LoginResponseDto();
            response.Email = loginRequestDto.Email;
            response.ExpiresAt = DateTime.UtcNow.AddMinutes(10);
            response.Token = null;

            return response;
        }
    }
}