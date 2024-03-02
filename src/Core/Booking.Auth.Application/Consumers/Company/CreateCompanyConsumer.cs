using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.Company;

public class CreateCompanyConsumer : IConsumer<CreateCompany>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    public CreateCompanyConsumer(ICompanyRepository companyRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<CreateCompany> context)
    {
        var request = context.Message;
        
        if (await _companyRepository.HasAnyWithInnAsync(request.Inn))
            throw new BadRequestException($"Company with INN {request.Inn} already exists");
            
        var company = _mapper.Map<Domain.Entities.Company>(request);
        
        await _companyRepository.CreateAsync(company);

        await context.RespondAsync(_mapper.Map<CreateCompanyResult>(company));
    }
}