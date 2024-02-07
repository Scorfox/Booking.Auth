using AutoMapper;
using Booking.Auth.Application.Common;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;
using Otus.Booking.Common.Booking.Contracts.Authentication.Responses;

namespace Booking.Auth.Application.Consumers.Client;

public class CreateClientConsumer : IConsumer<CreateClient>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateClientConsumer(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<CreateClient> context)
    {
        var request = context.Message;
        if (await _userRepository.HasAnyByEmailAsync(request.Email))
            throw new BadRequestException($"User with {request.Email} already exists");
            
        var user = _mapper.Map<User>(request);
        user.Role = (await _roleRepository.GetByNameAsync(Roles.Client))!;
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
        
        await _userRepository.Create(user);

        await context.RespondAsync(_mapper.Map<CreateClientResult>(user));
    }
}