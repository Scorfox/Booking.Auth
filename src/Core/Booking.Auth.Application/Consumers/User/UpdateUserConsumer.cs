using AutoMapper;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;
using Otus.Booking.Common.Booking.Contracts.Authentication.Responses;

namespace Booking.Auth.Application.Consumers.User;

public class UpdateUserConsumer : IConsumer<UpdateUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserConsumer(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<UpdateUser> context)
    {
        var request = context.Message;
        var user = await _userRepository.Get(request.Id);
        
        if (user == null)
            throw new BadRequestException($"User with ID {request.Id} doesn't exist");
        
        _mapper.Map(request, user);
        user.PasswordHash = new PasswordHasher<Domain.Entities.User>().HashPassword(user, request.Password);
        
        await _userRepository.Update(user);

        await context.RespondAsync(_mapper.Map<UpdateUserResult>(user));
    }
}