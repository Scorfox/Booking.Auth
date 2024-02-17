using AutoMapper;
using Booking.Auth.Application.Exceptions;
using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Application.Consumers.Filial;

public class UpdateFilialConsumer : IConsumer<UpdateFilial>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IFilialRepository _filialRepository;
    private readonly IMapper _mapper;

    public UpdateFilialConsumer(ICompanyRepository companyRepository, IFilialRepository filialRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _filialRepository = filialRepository;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<UpdateFilial> context)
    {
        var request = context.Message;
        var filial = await _filialRepository.FindByIdAsync(request.Id);
        
        if (filial == null)
            throw new NotFoundException($"Filial with ID {request.Id} doesn't exist");

        if (await _filialRepository.HasAnyWithNameExceptIdAsync(request.Id, request.Name))
            throw new BadRequestException($"Filial with NAME {request.Name} already exists");
                
        if (!await _companyRepository.HasAnyByIdAsync(request.CompanyId))
            throw new BadRequestException($"Company with ID {request.Name} doesn't exists");;
        
        _mapper.Map(request, filial);
        
        await _filialRepository.UpdateAsync(filial);

        await context.RespondAsync(_mapper.Map<UpdateFilialResult>(filial));
    }
}