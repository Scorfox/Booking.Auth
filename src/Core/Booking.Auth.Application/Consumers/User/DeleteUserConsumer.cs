using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Application.Consumers.User
{
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

            await _userRepository.DeleteUserAsync(request.Id);

            await context.RespondAsync(new DeleteUserResult());
        }
    }
}
