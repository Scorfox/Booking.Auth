using Booking.Auth.Application.Repositories;
using MassTransit;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Application.Consumers.Filial
{
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


            await _filialRepository.DeleteFilialByIdAsync(request.Id);

            await context.RespondAsync(new DeleteFilialResult());
        }
    }
}
