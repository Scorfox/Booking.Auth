using MassTransit;
using AutoMapper;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Application.Exceptions;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Application.Consumers
{
    public class GetUserConsumer  : IConsumer<GetUserId>
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

            if (!await _userRepository.HasAnyByIdAsync(request.Id, default))
                throw new BadRequestException($"User with ID {request.Id} doesn't exists");

            var user = await _userRepository.FindByIdAsync(false, request.Id);

            await context.RespondAsync(_mapper.Map<GetUserResult>(user));
        }
    }
}
