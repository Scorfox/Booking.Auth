using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Models;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Application.Consumers.Company
{
    public class GetCompaniesListConsumer:IConsumer<GetCompaniesList>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public GetCompaniesListConsumer(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetCompaniesList> context)
        {
            var request = context.Message;

            var companies = await _companyRepository.GetAllCompaniesAsync(request.Offset, request.Limit);

            await context.RespondAsync(new GetCompaniesListResult
                {Companies = companies.Select(elm => _mapper.Map<FullCompanyDto>(elm)).ToList()});
        }
    }
}
