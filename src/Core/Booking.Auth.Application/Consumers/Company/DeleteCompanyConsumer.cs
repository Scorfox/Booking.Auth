using AutoMapper;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;


namespace Booking.Auth.Application.Consumers.Company
{
    public class DeleteCompanyConsumer:IConsumer<DeleteCompany>
    {
        private readonly ICompanyRepository _companyRepository;

        public DeleteCompanyConsumer(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task Consume(ConsumeContext<DeleteCompany> context)
        {
            var request = context.Message;

            
            await _companyRepository.DeleteCompanyByIdAsync(request.Id);

            await context.RespondAsync(new DeleteCompanyResult());
        }
    }
}
