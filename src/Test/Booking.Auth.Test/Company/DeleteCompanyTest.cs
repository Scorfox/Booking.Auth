using AutoFixture;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

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
            // Arrange
            var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
            await DataContext.Companies.AddAsync(company);
            await DataContext.SaveChangesAsync();
            
            var testHarness = new InMemoryTestHarness();
            testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteCompany>();
            request.Id = company.Id;
            
            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteCompany>().Any(), Is.True);
                Assert.That(testHarness.Published.Select<DeleteCompanyResult>().Any(), Is.True);
            });

            await testHarness.Stop();
        }
    }
}
