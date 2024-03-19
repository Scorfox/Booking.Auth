using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Repositories;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Test.User;

public class CreateUserTests : BaseTest
{
    private CreateUserConsumer Consumer { get; }
    
    public CreateUserTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>());
        
        Consumer = new CreateUserConsumer(new UserRepository(DataContext), new Mapper(config));
    }

    [Test]
    public async Task CreateUser_ReturnsSuccess()
    {
        // Arrange
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(Fixture.Create<CreateUser>());
       
        // Assert
        Assert.Multiple(async () =>
        {
            Assert.That(testHarness.Consumed.Select<CreateUser>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<CreateUserResult>().Any(), Is.True);
            Assert.That(await DataContext.Users.AnyAsync(), Is.True);
        });
        
        await testHarness.Stop();
    }

    [Test]
    public async Task CreateUser_WithNotUniqueEmail_ReturnsException()
    {
        // Arrange
        const string email = "test@gmail.com";
        
        var user = Fixture.Build<Domain.Entities.User>()
                .With(e => e.Email, email)
                .Create();

        await DataContext.Users.AddAsync(user);
        await DataContext.SaveChangesAsync();

        var request = Fixture.Build<CreateUser>()
            .With(e => e.Email, email)
            .Create();
        
        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start(); 
        
        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);
       
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<CreateUser>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<Fault>().FirstOrDefault(), Is.Not.Null);
        });
        
        await testHarness.Stop();
    }
}