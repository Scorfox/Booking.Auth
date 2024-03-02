using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Domain.Entities;
using Booking.Auth.Persistence.Context;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            for (int i = 0; i < 5; i++)
            {
                var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
                await DataContext.Companies.AddAsync(company);
            }

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<GetCompaniesList>();
            request.Offset = 0;
            request.Limit = 3;
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<GetCompaniesListResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<GetCompaniesList>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<GetCompaniesList>().Any(), Is.True);
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Companies.Count,Is.EqualTo(3));
            });

            await testHarness.Stop();
        }
    }
}
