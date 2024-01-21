using AutoMapper;
using Booking.Auth.Application.Common;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Booking.Auth.Application.Features.UserFeatures;

public sealed class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (!Roles.GetAllRolesWithIds().Keys.Contains(request.RoleName))
            throw new NotFoundException($"Role with name {request.RoleName} not found");
        
        if (await _userRepository.HasAnyByEmailAsync(request.Email, cancellationToken))
            throw new BadRequestException($"User with {request.Email} already exists");
            
        var user = _mapper.Map<User>(request);
        
        user.RoleId = Roles.GetAllRolesWithIds()[Roles.Admin];
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
        
        await _userRepository.Create(user);

        return _mapper.Map<CreateUserResponse>(user);
    }
}