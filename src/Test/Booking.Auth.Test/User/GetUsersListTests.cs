using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Domain.Entities;
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

        Consumer = new GetUsersListConsumers(new UserRepository(DataContext), new Mapper(config));
    }

    [Test]
    public async Task GetUsersListTest()
    {
        // Arrange
        var users = new List<Domain.Entities.User>();
        var roleIdByName = Application.Common.Roles.GetAllRolesWithIds();
        for (var i = 0; i < 5; i++)
            users.Add(Fixture.Build<Domain.Entities.User>()
                .Without(e => e.Role)
                .With(e => e.RoleId, roleIdByName[Application.Common.Roles.Admin])
                .Create());

        await DataContext.Users.AddRangeAsync(users);
        await DataContext.SaveChangesAsync();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start();

        var request = Fixture.Build<GetUsersList>()
            .With(e => e.Offset, 0)
            .With(e => e.Count, 3)
            .With(e => e.RoleId, roleIdByName[Application.Common.Roles.Admin])
            .Create();
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<GetUsersListResult>().FirstOrDefault()?.Context.Message;
        var count = await DataContext.Users.CountAsync();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetUsersList>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<GetUsersListResult>().Any(), Is.True);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Elements.Count, Is.EqualTo(3));
            Assert.That(result.TotalCount, Is.EqualTo(count));
        });

        await testHarness.Stop();
    }
}