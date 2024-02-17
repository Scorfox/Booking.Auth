using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Test.Company;

public class UpdateCompanyTests : BaseTest
{
    private UpdateCompanyConsumer Consumer { get; }
    
    public UpdateCompanyTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMapper>());
        
        var companyRepository = new CompanyRepository(DataContext);
        Consumer = new UpdateCompanyConsumer(companyRepository, new Mapper(config));
    }

    [Test]
    public async Task UpdateCompany_ReturnsSuccess()
    {
        // Arrange
        var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
        await DataContext.Companies.AddAsync(company);
        await DataContext.SaveChangesAsync();
        
        var request = Fixture.Create<UpdateCompany>();
        request.Id = company.Id;
        
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<UpdateCompanyResult>().FirstOrDefault()?.Context.Message;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateCompany>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<UpdateCompany>().Any(), Is.True);
            Assert.That(company.Inn, Is.EqualTo(result?.Inn));
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task UpdateNotCreatedCompany_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);

        var request = Fixture.Create<UpdateCompany>();
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<UpdateCompany>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}