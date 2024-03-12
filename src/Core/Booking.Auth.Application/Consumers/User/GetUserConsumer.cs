using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.User
{
    public sealed class GetUserConsumer : IConsumer<GetUserById>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserConsumer(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUserById> context)
        {
            var request = context.Message;

            var user = _userRepository.FindByIdAsync(request.Id);

            if (user == null)
                throw new NotFoundException($"User with ID {request.Id} doesn't exists");

            await context.RespondAsync(_mapper.Map<GetUserResult>(user));
        }
    }
}
