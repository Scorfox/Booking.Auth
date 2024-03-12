using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Test.Company
{
    public class GetCompaniesListTests:BaseTest
    {
        private GetCompaniesListConsumer Consumer { get; }

        public GetCompaniesListTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMapper>());

            var companyRepository = new CompanyRepository(DataContext);
            Consumer = new GetCompaniesListConsumer(companyRepository, new Mapper(config));
        }

        [Test]
        public async Task TestGetCompaniesList()
        {
            // Arrange
            var companies = new List<Domain.Entities.Company>();
            
            for (var i = 0; i < 5; i++)
                companies.Add(Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create());
            
            await DataContext.Companies.AddRangeAsync(companies);
            await DataContext.SaveChangesAsync();
            
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<GetCompaniesList>();
            request.Offset = 0;
            request.Count = 3;
            
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<GetCompaniesListResult>().FirstOrDefault()?.Context.Message;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetCompaniesList>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetCompaniesList>().Any(), Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Elements.Count, Is.EqualTo(3));
            });

            await testHarness.Stop();
        }
    }
}
