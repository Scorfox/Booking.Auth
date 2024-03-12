using AutoMapper;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.Filial;

public class CreateFilialConsumer : IConsumer<CreateFilial>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IFilialRepository _filialRepository;
    private readonly IMapper _mapper;

    public CreateFilialConsumer(ICompanyRepository companyRepository, IFilialRepository filialRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _filialRepository = filialRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<CreateFilial> context)
    {
        var request = context.Message;
        
        if (await _filialRepository.HasAnyWithNameAsync(request.Name))
            throw new BadRequestException($"Filial with NAME {request.Name} already exists");
        
        if (!await _companyRepository.HasAnyByIdAsync(request.CompanyId))
            throw new BadRequestException($"Company with ID {request.CompanyId} doesn't exists");
            
        var filial = _mapper.Map<Domain.Entities.Filial>(request);
        
        await _filialRepository.CreateAsync(filial);

        await context.RespondAsync(_mapper.Map<CreateFilialResult>(filial));
    }
}