using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Test.Filial;

public class GetFilialTests : BaseTest
{
    private GetFilialConsumer Consumer { get; }

    public GetFilialTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());

        Consumer = new GetFilialConsumer(new FilialRepository(DataContext), new Mapper(config));
    }

    [Test]
    public async Task GetFilialByIdTest_ReturnsSuccess()
    {
        // Arrange
        var filial = Fixture.Build<Domain.Entities.Filial>().Without(e => e.Company).Create();
        var company = Fixture.Build<Domain.Entities.Company>()
            .With(e => e.Filials, [filial])
            .Create();
        await DataContext.AddAsync(company);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Build<GetFilialById>()
            .With(e => e.Id, filial.Id)
            .Create();

        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start();

        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetFilialById>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<GetFilialResult>().Any(), Is.True);
        });

        await testHarness.Stop();
    }
}