using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using AutoMapper;
using Booking.Auth.Application.Repositories;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.Company
{
    public class GetCompanyConsumer : IConsumer<GetCompanyId>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public GetCompanyConsumer(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<GetCompanyId> context)
        {
            var request = context.Message;

            if (!await _companyRepository.HasAnyByIdAsync(request.Id))
                throw new NotFoundException($"Company with ID {request.Id} doesn't exists");

            await context.RespondAsync(_companyRepository.FindByIdAsync(request.Id, default));
        }
    }
}
