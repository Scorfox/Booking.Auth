using AutoMapper;
using Booking.Auth.Application.Common;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Booking.Auth.Application.Features.ClientFeatures;

public sealed class CreateClientHandler : IRequestHandler<CreateClientRequest, CreateClientResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateClientHandler(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task<CreateClientResponse> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        if (await _userRepository.HasAnyByEmailAsync(request.Email, cancellationToken))
            throw new BadRequestException($"User with {request.Email} already exists");
            
        var user = _mapper.Map<User>(request);
        user.Role = (await _roleRepository.GetByNameAsync(Roles.Client, cancellationToken))!;
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
        
        await _userRepository.CreateAsync(user);

        return _mapper.Map<CreateClientResponse>(user);
    }
}