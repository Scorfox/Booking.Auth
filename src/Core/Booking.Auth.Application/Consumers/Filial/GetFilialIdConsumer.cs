using AutoMapper;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Auth.Application.Consumers.Filial
{
    public sealed class GetFilialIdConsumer : IConsumer<GetFilialId>
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IMapper _mapper;

        public GetFilialIdConsumer(IFilialRepository filialRepository, IMapper mapper)
        {
            _filialRepository = filialRepository;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetFilialId> context)
        {
            var request = context.Message;

            if (!await _filialRepository.HasAnyWithIdAsync(request.Id))
                throw new BadRequestException($"Filail with ID {request.Id} doesn't exists");

            await context.RespondAsync(_filialRepository.FindByIdAsync(request.Id, default));
        }
    }
}
