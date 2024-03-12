using Booking.Auth.Persistence.Repositories;
using Booking.Auth.Application.Consumers.Filial;
using AutoFixture;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Test.Filial
{
    public class DeleteFilialTests: BaseTest
    {
        private DeleteFilialConsumer Consumer;

        public DeleteFilialTests()
        {
            var filialRepository = new FilialRepository(DataContext);
            Consumer = new DeleteFilialConsumer(filialRepository);
        }

        [Test]
        public async Task DeleteFilialTest()
        {
            // Acr
            var filial = Fixture.Build<Domain.Entities.Filial>().Without(e => e.Company).Create();
            await DataContext.Filials.AddAsync(filial);
            await DataContext.SaveChangesAsync();
            
            var testHarness = new InMemoryTestHarness();
            testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteFilial>();
            request.Id = filial.Id;

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteFilial>().Any(), Is.True);
                Assert.That(testHarness.Published.Select<DeleteFilialResult>().Any(), Is.True);
            });

            await testHarness.Stop();

        }

    }
}
