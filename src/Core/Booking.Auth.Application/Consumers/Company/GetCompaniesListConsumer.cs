using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.Booking.Common.Booking.Contracts.Company.Models;

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

            var companies = await _companyRepository.GetAllCompaniesAsync(request.Page, request.PageSize);

            await context.RespondAsync(companies.Select(elm => _mapper.Map<FullCompanyDto>(elm)));
        }
    }
}
