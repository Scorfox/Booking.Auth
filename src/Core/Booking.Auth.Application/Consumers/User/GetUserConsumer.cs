using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.User;

public sealed class GetUserConsumer : IConsumer<GetUserById>
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public GetUserConsumer(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<GetUserById> context)
    {
        var request = context.Message;

        var user = await _userRepository.FindByIdAsync(request.Id);

        if (user == null)
            throw new NotFoundException($"User with ID {request.Id} doesn't exists");

        await context.RespondAsync(_mapper.Map<GetUserResult>(user));
    }
}