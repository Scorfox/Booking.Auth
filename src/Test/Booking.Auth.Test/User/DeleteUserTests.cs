using AutoFixture;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Test.User;

public class DeleteUserTests:BaseTest
{
    private DeleteUserConsumer Consumer { get; }

    public DeleteUserTests()
    {
        var userRepository = new UserRepository(DataContext);
        Consumer = new DeleteUserConsumer(userRepository);
    }

    [Test]
    public async Task DeleteFilialTest()
    {
        // Arrange
        var user = Fixture.Build<Domain.Entities.User>().Create();
        await DataContext.Users.AddAsync(user);
        await DataContext.SaveChangesAsync();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);

        await testHarness.Start();

        var request = Fixture.Create<DeleteUser>();
        request.Id = user.Id;

        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<DeleteUser>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<DeleteUserResult>().Any(), Is.True);
        });

        await testHarness.Stop();
    }
}