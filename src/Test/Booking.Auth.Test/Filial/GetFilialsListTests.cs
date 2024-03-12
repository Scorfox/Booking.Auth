﻿using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Filial;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;

namespace Booking.Auth.Test.Filial;

public class GetFilialsListTests:BaseTest
{
    private GetFilialsListConsumer Consumer { get; }

    public GetFilialsListTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<FilialMapper>());
            
        var filialRepository = new FilialRepository(DataContext);
        Consumer = new GetFilialsListConsumer(filialRepository, new Mapper(config));
    }

    [Test]
    public async Task GetFilialsListTest()
    {
        // Arrange
        var filials = new List<Domain.Entities.Filial>();
                
        for (var i = 0; i < 5; i++)
            filials.Add(Fixture.Build<Domain.Entities.Filial>().Without(e => e.Company).Create());
            
        await DataContext.Filials.AddRangeAsync(filials);
        await DataContext.SaveChangesAsync();
            
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);

        await testHarness.Start();

        var request = Fixture.Create<GetFilialsList>();
        request.Offset = 0;
        request.Count = 3;
            
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<GetFilialsListResult>().FirstOrDefault()?.Context.Message;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetFilialsList>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<GetFilialsListResult>().Any(), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Elements.Count, Is.EqualTo(3));
        });

        await testHarness.Stop();
    }
}