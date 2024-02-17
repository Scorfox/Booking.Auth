using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;

namespace Booking.Auth.Test.Company;

public class CreateCompanyTests : BaseTest
{
    private CreateCompanyConsumer Consumer { get; }
    
    public CreateCompanyTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMapper>());
        
        var companyRepository = new CompanyRepository(DataContext);
        Consumer = new CreateCompanyConsumer(companyRepository, new Mapper(config));
    }

    [Test]
    public async Task CreateCompany_ReturnsSuccess()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(Fixture.Create<CreateCompany>());
       
        // Assert
        Assert.Multiple(async () =>
        {
            Assert.That(testHarness.Consumed.Select<CreateCompany>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<CreateCompany>().Any(), Is.True);
            Assert.That(await DataContext.Companies.AnyAsync(), Is.True);
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task CreateCompany_WithNotUniqueInn_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        const string inn = "123";
        
        var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
        company.Inn = inn;

        await DataContext.Companies.AddAsync(company);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Create<CreateCompany>();
        request.Inn = inn;
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<CreateCompany>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}