using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.User;

public class DeleteUserConsumer:IConsumer<DeleteUser>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<DeleteUser> context)
    {
        var request = context.Message;

        var user = await _userRepository.FindByIdAsync(request.Id);
        
        if (user == null)
            throw new NotFoundException($"User with ID {request.Id} doesn't exists");
        
        await _userRepository.Delete(user);

        await context.RespondAsync(new DeleteUserResult());
    }
}