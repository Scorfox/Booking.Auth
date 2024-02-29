using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;

namespace Booking.Auth.Test.Company
{
    public class GetCompanyTest : BaseTest
    {
        private GetCompanyConsumer Consumer { get; }

        public GetCompanyTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMapper>());

            var companyRepository = new CompanyRepository(DataContext);
            Consumer = new GetCompanyConsumer(companyRepository, new Mapper(config));
        }

        [Test]
        public async Task GetCompanyByIdTest_ReturnsSuccess()
        {
            // Arrange
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);
            Guid id = Guid.NewGuid();

            var request = Fixture.Create<GetCompanyId>();
            request.Id = id;

            await testHarness.Start();

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetCompanyId>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetCompanyId>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
