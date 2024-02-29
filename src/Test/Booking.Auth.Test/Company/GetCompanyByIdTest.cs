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
    public class GetCompanyByIdTest : BaseTest
    {
        private GetCompanyIdConsumer Consumer { get; }

        public GetCompanyByIdTest()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMapper>());

            var companyRepository = new CompanyRepository(DataContext);
            Consumer = new GetCompanyIdConsumer(companyRepository, new Mapper(config));
        }

        [Test]
        public async Task GetCompanyByIdTest_ReturnsSuccess()
        {
            // Arrange
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);
            Guid id = Guid.NewGuid();

            var request = Fixture.Create<GetCompanieId>();
            request.Id = id;

            await testHarness.Start();

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetCompanieId>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetCompanieId>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
