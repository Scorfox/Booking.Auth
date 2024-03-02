﻿using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.User;

public class CreateUserConsumer : IConsumer<CreateUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserConsumer(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<CreateUser> context)
    {
        var request = context.Message;
        
        if (await _userRepository.HasAnyByEmailAsync(request.Email))
            throw new BadRequestException($"User with {request.Email} already exists");
            
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.PasswordHash = new PasswordHasher<Domain.Entities.User>().HashPassword(user, request.Password);
        
        await _userRepository.CreateAsync(user);

        await context.RespondAsync(_mapper.Map<CreateUserResult>(user));
    }
}