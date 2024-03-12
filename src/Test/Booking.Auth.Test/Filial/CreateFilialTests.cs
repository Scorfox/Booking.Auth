using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Test.Filial;

public class CreateFilialTests : BaseTest
{
    private CreateFilialConsumer Consumer { get; }
    private ICompanyRepository CompanyRepository { get; }
    
    public CreateFilialTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());
        
        CompanyRepository = new CompanyRepository(DataContext);
        var filialRepository = new FilialRepository(DataContext);
        Consumer = new CreateFilialConsumer(CompanyRepository, filialRepository, new Mapper(config));
    }

    [Test]
    public async Task CreateFilial_ReturnsSuccess()
    {
        // Arrange
        var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
        await CompanyRepository.CreateAsync(company);
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        
        await testHarness.Start();

        var request = Fixture.Create<CreateFilial>();
        request.CompanyId = company.Id;
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(async () =>
        {
            Assert.That(testHarness.Consumed.Select<CreateFilial>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<CreateFilialResult>().Any(), Is.True);
            Assert.That(await DataContext.Filials.AnyAsync(), Is.True);
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task CreateFilial_WithNotUniqueName_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        const string name = "FilialName";
        
        var filial = Fixture.Build<Domain.Entities.Filial>().With(e => e.Company, 
            Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create)
            .Create();
        filial.Name = name;

        await DataContext.Filials.AddAsync(filial);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Create<CreateFilial>();
        request.Name = name;
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<CreateFilial>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
        });
        
        await testHarness.Stop();
    }
}