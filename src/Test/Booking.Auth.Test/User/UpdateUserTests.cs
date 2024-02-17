using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Test.User;

public class UpdateUserTests : BaseTest
{
    private UpdateUserConsumer Consumer { get; }
    
    public UpdateUserTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>());
        
        var userRepository = new UserRepository(DataContext);
        Consumer = new UpdateUserConsumer(userRepository, new Mapper(config));
    }

    [Test]
    public async Task UpdateUser_ReturnsSuccess()
    {
        // Arrange
        var user = Fixture.Create<Domain.Entities.User>();
        await DataContext.Users.AddAsync(user);
        await DataContext.SaveChangesAsync();
        
        var request = Fixture.Create<UpdateUser>();
        request.Id = user.Id;
        
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
        var result = testHarness.Published.Select<UpdateUserResult>().FirstOrDefault()?.Context.Message;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<UpdateUser>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<UpdateUser>().Any(), Is.True);
            Assert.That(user.LastName, Is.EqualTo(result?.LastName));
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task UpdateNotCreatedUser_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);

        var request = Fixture.Create<UpdateUser>();
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<UpdateUser>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}