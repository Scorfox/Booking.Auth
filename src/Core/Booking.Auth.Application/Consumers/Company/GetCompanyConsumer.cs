using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using AutoMapper;
using Booking.Auth.Application.Repositories;
using Otus.Booking.Common.Booking.Contracts.Filial.Models;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Booking.Auth.Application.Exceptions;

namespace Booking.Auth.Application.Consumers.Company
{
    public class GetCompanyConsumer : IConsumer<GetCompanieId>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public GetCompanyConsumer(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetCompanieId> context)
        {
            var request = context.Message;

            if (!await _companyRepository.HasAnyByIdAsync(request.Id))
                throw new BadRequestException($"Company with ID {request.Id} doesn't exists");

            await context.RespondAsync(_companyRepository.FindByIdAsync(request.Id, default));
        }
    }
}
