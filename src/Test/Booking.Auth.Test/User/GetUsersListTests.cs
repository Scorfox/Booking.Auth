using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Test.User;

public class GetUsersListTests : BaseTest
{
    private GetUsersListConsumers Consumer { get; }
    
    public GetUsersListTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>());

        var userRepository = new UserRepository(DataContext);
        Consumer = new GetUsersListConsumers(userRepository, new Mapper(config));
    }

    [Test]
    public async Task GetUsersListTest()
    {
        // Arrange
        var users = new List<Domain.Entities.User>();
        
        for (var i = 0; i < 5; i++)
            users.Add(Fixture.Build<Domain.Entities.User>().Create());
        
        await DataContext.Users.AddRangeAsync(users);

        await DataContext.SaveChangesAsync();
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);

        await testHarness.Start();

        var request = Fixture.Create<GetUsersList>();
        request.Offset = 0;
        request.Count = 3;
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<GetUsersListResult>().FirstOrDefault()?.Context.Message;
        var count = await DataContext.Users.CountAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetUsersList>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<GetUsersList>().Any(), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Elements.Count, Is.EqualTo(3));
            Assert.That(result.TotalCount, Is.EqualTo(count));
        });

        await testHarness.Stop();
    }
}