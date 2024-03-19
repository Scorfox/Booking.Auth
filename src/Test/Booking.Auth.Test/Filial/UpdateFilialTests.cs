using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Test.Filial;

public class UpdateFilialTests : BaseTest
{
    private UpdateFilialConsumer Consumer { get; }
    
    public UpdateFilialTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());
        
        Consumer = new UpdateFilialConsumer(new CompanyRepository(DataContext), new FilialRepository(DataContext), new Mapper(config));
    }

    [Test]
    public async Task UpdateFilial_ReturnsSuccess()
    {
        // Arrange
        var filial = Fixture.Build<Domain.Entities.Filial>()
            .With(e => e.Company, Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create)
            .Create();
        await DataContext.Filials.AddAsync(filial);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Build<UpdateFilial>()
            .With(e => e.Id, filial.Id)
            .With(e => e.CompanyId, filial.CompanyId)
            .Create();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<UpdateFilialResult>().FirstOrDefault()?.Context.Message;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateFilial>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<UpdateFilialResult>().Any(), Is.True);
            Assert.That(filial.Name, Is.EqualTo(result?.Name));
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task UpdateNotCreatedFilial_ReturnsException()
    {
        // Arrange
        var request = Fixture.Create<UpdateFilial>();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(testHarness.Consumed.Select<UpdateFilial>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}