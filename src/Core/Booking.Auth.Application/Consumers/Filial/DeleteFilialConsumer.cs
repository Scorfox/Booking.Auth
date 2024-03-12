using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Exceptions;

namespace Booking.Auth.Application.Consumers.Filial;

public class DeleteFilialConsumer:IConsumer<DeleteFilial>
{
    private readonly IFilialRepository _filialRepository;

    public DeleteFilialConsumer(IFilialRepository filialRepository)
    {
        _filialRepository = filialRepository;
    }

    public async Task Consume(ConsumeContext<DeleteFilial> context)
    {
        var request = context.Message;

        var filial = await _filialRepository.FindByIdAsync(request.Id);
        
        if (filial == null)
            throw new NotFoundException($"Filial with ID {request.Id} doesn't exists");
        
        await _filialRepository.Delete(filial);

        await context.RespondAsync(new DeleteFilialResult());
    }
}