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
    public class DeleteCompanyTest:BaseTest
    {
        private DeleteCompanyConsumer Consumer;

        public DeleteCompanyTest()
        {
            var companyRepository = new CompanyRepository(DataContext);
            Consumer = new DeleteCompanyConsumer(companyRepository);
        }


        [Test]
        public async Task TestDeleteCompany()
        {
            var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
            await DataContext.Companies.AddAsync(company);

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteCompany>();
            request.Id = company.Id;
            
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<DeleteCompanyResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteCompany>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<DeleteCompany>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
