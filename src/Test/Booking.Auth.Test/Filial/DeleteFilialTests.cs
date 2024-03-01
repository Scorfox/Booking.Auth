using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Auth.Application.Consumers.Filial;
using AutoFixture;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
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
            var filial = Fixture.Build<Domain.Entities.Filial>().Without(e => e.Company).Create();
            await DataContext.Filials.AddAsync(filial);

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteFilial>();
            request.Id = filial.Id;

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<DeleteFilialResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteFilial>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<DeleteFilial>().Any(), Is.True);
            });

            await testHarness.Stop();

        }

    }
}
