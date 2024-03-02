using AutoMapper;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.User.Requests;

namespace Booking.Auth.Application.Consumers.User
{
    public sealed class GetUserConsumer : IConsumer<GetUserId>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserConsumer(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetUserId> context)
        {
            var request = context.Message;

            if (!await _userRepository.HasAnyByIdAsync(request.Id))
                throw new NotFoundException($"User with ID {request.Id} doesn't exists");

            await context.RespondAsync(_userRepository.FindByIdAsync(request.Id, default));
        }
    }
}
