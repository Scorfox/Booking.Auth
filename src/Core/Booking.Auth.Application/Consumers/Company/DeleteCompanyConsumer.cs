using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.Company;

public class DeleteCompanyConsumer : IConsumer<DeleteCompany>
{
    private readonly ICompanyRepository _companyRepository;

    public DeleteCompanyConsumer(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task Consume(ConsumeContext<DeleteCompany> context)
    {
        var request = context.Message;

        var company = await _companyRepository.FindByIdAsync(request.Id);
        
        if (company == null)
            throw new NotFoundException($"Company with ID {request.Id} doesn't exists");
        
        await _companyRepository.Delete(company);

        await context.RespondAsync(new DeleteCompanyResult());
    }
}