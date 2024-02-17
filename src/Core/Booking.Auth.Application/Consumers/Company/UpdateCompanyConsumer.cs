using AutoMapper;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Application.Consumers.Company;

public class UpdateCompanyConsumer : IConsumer<UpdateCompany>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public UpdateCompanyConsumer(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<UpdateCompany> context)
    {
        var request = context.Message;
        var company = await _companyRepository.FindByIdAsync(request.Id);
        
        if (company == null)
            throw new NotFoundException($"Company with ID {request.Id} doesn't exist");
        
        if (await _companyRepository.HasAnyWithInnExceptIdAsync(request.Id, request.Inn))
            throw new BadRequestException($"Company with INN {request.Inn} already exists");
        
        _mapper.Map(request, company);
        
        await _companyRepository.UpdateAsync(company);

        await context.RespondAsync(_mapper.Map<UpdateCompanyResult>(company));
    }
}