using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Test.Filial
{
    public class GetFilialsListTests:BaseTest
    {
        private GetFilialsListConsumer Consumer { get; }

        public GetFilialsListTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());
            
            var filialRepository = new FilialRepository(DataContext);
            Consumer = new GetFilialsListConsumer(filialRepository, new Mapper(config));
        }

        [Test]
        public async Task GetFilialsListTest()
        {
            for (int i = 0; i < 5; i++)
            {
                var filial = Fixture.Build<Domain.Entities.Filial>().Without(e => e.Company).Create();
                
                await DataContext.Filials.AddAsync(filial);
            }

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<GetFilialsList>();
            request.Offset = 0;
            request.Count = 3;
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<GetFilialsListResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetFilialsList>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetFilialsList>().Any(), Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Elements.Count, Is.EqualTo(3));
            });

            await testHarness.Stop();
        }
    }
}
