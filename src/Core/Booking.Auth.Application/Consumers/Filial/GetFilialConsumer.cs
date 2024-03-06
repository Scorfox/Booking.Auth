using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.Filial
{
    public sealed class GetFilialConsumer : IConsumer<GetFilialId>
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IMapper _mapper;

        public GetFilialConsumer(IFilialRepository filialRepository, IMapper mapper)
        {
            _filialRepository = filialRepository;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetFilialId> context)
        {
            var request = context.Message;

            var filial = _filialRepository.FindByIdAsync(request.Id);

            if (filial == null)
                throw new NotFoundException($"Filial with ID {request.Id} doesn't exists");

            await context.RespondAsync(_mapper.Map<GetFilialResult>(filial));
        }
    }
}
