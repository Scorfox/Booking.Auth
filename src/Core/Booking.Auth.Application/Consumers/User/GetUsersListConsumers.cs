using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.User.Models;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Application.Consumers.User;

public class GetUsersListConsumers:IConsumer<GetUsersList>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUsersListConsumers(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<GetUsersList> context)
    {
        var request = context.Message;

        var users = await _userRepository.GetPaginatedListAsync(request.Offset, request.Count);
        var totalCount = await _userRepository.GetTotalCount();

        await context.RespondAsync(new GetUsersListResult
        {
            Elements = _mapper.Map<List<FullUserDto>>(users), TotalCount = totalCount
        });
    }
}