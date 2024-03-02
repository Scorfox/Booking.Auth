using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Models;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Application.Consumers.Filial
{
    public class GetFilialsListConsumer:IConsumer<GetFilialsList>
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IMapper _mapper;

        public GetFilialsListConsumer(IFilialRepository filialRepository, IMapper mapper)
        {
            _filialRepository = filialRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetFilialsList> context)
        {
            var request = context.Message;

            var filials = await _filialRepository.GetFilialsListAsync(request.Offset, request.Limit);

            var result = new GetFilialsListResult
                {Filials = filials.Select(elm => _mapper.Map<FullFilialDto>(elm)).ToList()};

            await context.RespondAsync(result);
        }
    }
}
