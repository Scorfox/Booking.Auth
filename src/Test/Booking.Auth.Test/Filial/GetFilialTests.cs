using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;

namespace Booking.Auth.Test.Filial
{
    public class GetFilialTests : BaseTest
    {
        private GetFilialIdConsumer Consumer { get; }

        public GetFilialTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());

            var filialRepository = new FilialRepository(DataContext);
            Consumer = new GetFilialIdConsumer(filialRepository, new Mapper(config));
        }

        [Test]
        public async Task GetCompanyByIdTest_ReturnsSuccess()
        {
            // Arrange
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);
            Guid id = Guid.NewGuid();

            var request = Fixture.Create<GetFilialId>();
            request.Id = id;

            await testHarness.Start();

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetFilialId>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetFilialId>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
