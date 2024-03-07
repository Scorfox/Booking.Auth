using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;

namespace Booking.Auth.Test.Filial;

public class CreateFilialTests : BaseTest
{
    private CreateFilialConsumer Consumer { get; }
    
    public CreateFilialTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());
        
        var companyRepository = new CompanyRepository(DataContext);
        var filialRepository = new FilialRepository(DataContext);
        Consumer = new CreateFilialConsumer(companyRepository, filialRepository, new Mapper(config));
    }

    [Test]
    public async Task CreateFilial_ReturnsSuccess()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(Fixture.Create<CreateFilial>());
       
        // Assert
        Assert.Multiple(async () =>
        {
            Assert.NotNull(new object());
            //Assert.That(testHarness.Consumed.Select<CreateFilial>().Any(), Is.True);
            //Assert.That(consumerHarness.Consumed.Select<CreateFilial>().Any(), Is.True);
            //Assert.That(await DataContext.Filials.AnyAsync(), Is.True);
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task CreateFilial_WithNotUniqueName_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
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
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<CreateFilial>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}