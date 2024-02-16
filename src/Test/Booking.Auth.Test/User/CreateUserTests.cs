using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.Authentication.Requests;

namespace Booking.Auth.Test.User;

public class CreateUserTests : BaseTest
{
    private CreateUserConsumer Consumer { get; }
    
    public CreateUserTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>());
        
        var userRepository = new UserRepository(DataContext);
        Consumer = new CreateUserConsumer(userRepository, new Mapper(config));
    }

    [Test]
    public async Task CreateUser_ReturnsSuccess()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(Fixture.Create<CreateUser>());
       
        // Assert
        Assert.Multiple(async () =>
        {
            Assert.That(testHarness.Consumed.Select<CreateUser>().Any(), Is.True);
            Assert.That(consumerHarness.Consumed.Select<CreateUser>().Any(), Is.True);
            Assert.That(await DataContext.Users.AnyAsync(), Is.True);
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task CreateUser_WithNotUniqueEmail_ReturnsException()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        var consumerHarness = testHarness.Consumer(() => Consumer);
        const string email = "test@gmail.com";
        
        var user = Fixture.Create<Domain.Entities.User>();
        user.Email = email;

        await DataContext.Users.AddAsync(user);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Create<CreateUser>();
        request.Email = email;
        
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
            Assert.That(consumerHarness.Consumed.Select<CreateUser>().Any(), Is.True);
        });
        
        await testHarness.Stop();
    }
}