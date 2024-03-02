using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;
using Otus.Booking.Common.Booking.Exceptions;

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
        var user = await _userRepository.FindByIdAsync(request.Id);
        
        if (user == null)
            throw new NotFoundException($"User with ID {request.Id} doesn't exist");
        
        if (await _userRepository.HasAnyByEmailExceptIdAsync(request.Id, request.Email))
            throw new BadRequestException($"User with {request.Email} already exists");
        
        _mapper.Map(request, user);
        user.PasswordHash = new PasswordHasher<Domain.Entities.User>().HashPassword(user, request.Password);
        
        await _userRepository.UpdateAsync(user);

        await context.RespondAsync(_mapper.Map<UpdateUserResult>(user));
    }
}