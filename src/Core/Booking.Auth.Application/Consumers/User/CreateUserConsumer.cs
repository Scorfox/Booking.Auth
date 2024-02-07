using AutoMapper;
using Booking.Auth.Application.Common;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;
using Otus.Booking.Common.Booking.Contracts.Authentication.Responses;

namespace Booking.Auth.Application.Consumers.User;

public class CreateUserConsumer : IConsumer<CreateUser>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserConsumer(IRoleRepository roleRepository, IUserRepository userRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        var request = context.Message;
        if (await _userRepository.HasAnyByEmailAsync(request.Email))
            throw new BadRequestException($"User with {request.Email} already exists");
            
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Role = (await _roleRepository.GetByNameAsync(Roles.Client))!;
        user.PasswordHash = new PasswordHasher<Domain.Entities.User>().HashPassword(user, request.Password);
        
        await _userRepository.Create(user);

        await context.RespondAsync(_mapper.Map<CreateUserResult>(user));
    }
}