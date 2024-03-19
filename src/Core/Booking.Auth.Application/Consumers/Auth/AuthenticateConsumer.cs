using Booking.Auth.Application.Repositories;
using Booking.Auth.Application.Services;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Otus.Booking.Common.Booking.Contracts.Auth.Requests;
using Otus.Booking.Common.Booking.Exceptions;
using AuthenticateResult = Otus.Booking.Common.Booking.Contracts.Auth.Responses.AuthenticateResult;

namespace Booking.Auth.Application.Consumers.Auth;

public class AuthenticateConsumer : IConsumer<Authenticate>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticateConsumer(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }
    
    public async Task Consume(ConsumeContext<Authenticate> context)
    {
        var request = context.Message;
        var user = await _userRepository.FindByEmailAsync(true, request.Email);

        if (user == null)
            throw new NotFoundException($"User with email {request.Email} not found");
        
        var result = new PasswordHasher<Domain.Entities.User>().VerifyHashedPassword
            (user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new WrongPasswordException($"User with {request.Email} sent wrong password");
        
        await context.RespondAsync(new AuthenticateResult
        {
            AccessToken = _jwtTokenGenerator.GenerateToken(user.Email, user.Role.Name, user.CompanyId)
        });
    }
}