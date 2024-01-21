using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Booking.Auth.Application.Features.AuthFeatures;

public sealed class AuthenticateHandler : IRequestHandler<AuthenticateRequest, (bool success, string? roleName)>
{
    private readonly IUserRepository _userRepository;

    public AuthenticateHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<(bool success, string? roleName)> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(false, request.Email, cancellationToken);

        if (user == null)
            throw new NotFoundException($"User with email {request.Email} not found");
        
        var result = new PasswordHasher<User>().VerifyHashedPassword
            (user, user.PasswordHash, request.Password);

        return (result == PasswordVerificationResult.Success, user?.Role.Name);
    }
}