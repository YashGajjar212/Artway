using Artway.Application.Interfaces.Authentication;
using Artway.Application.Interfaces.Customers;
using Artway.DTOs.Auth;
using Artway.Models;
using Microsoft.AspNetCore.Identity;

namespace Artway.Application.Services.Authentication
{
    public class AuthService : IAuthService
    {
        //private readonly IAuthRepository _authRepository;
        private readonly ICustomerServices _customerServices;

        private readonly IPasswordHasher<Customer> _passwordHasher;

        public AuthService(IPasswordHasher<Customer> passwordHasher, ICustomerServices customerServices) // IAuthRepository authRepository, 
        {
            //_authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _customerServices = customerServices;
        }

        public async Task<RegisterResponseDto> RegisterCustomer(RegisterRequestDto registerRequestDto)
        {
            var existingCustomer = await _customerServices.GetCustomerByEmail(registerRequestDto.Email);

            if (existingCustomer != null)
                throw new Exception($"Customer already exists with email: {registerRequestDto.Email}");

            Customer newCustomer = new Customer();
            newCustomer.Email = registerRequestDto.Email;
            //newCustomer.PasswordHash = registerRequestDto.Password;
            newCustomer.PasswordHash = _passwordHasher.HashPassword(newCustomer, registerRequestDto.Password);

            var result = await _customerServices.AddCustomer(newCustomer);

            if (result == null)
                throw new Exception($"Unable to add new customer with email: {newCustomer.Email}");

            RegisterResponseDto registerResponseDto = new RegisterResponseDto();
            registerResponseDto.CustomerId = result.CustomerId;
            registerResponseDto.Email = result.Email;

            return registerResponseDto;
        }

        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            return null;
        }
    }
}