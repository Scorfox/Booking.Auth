using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.User;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Persistence.Context;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Filial.Requests;
using Otus.Booking.Common.Booking.Contracts.Filial.Responses;
using Otus.Booking.Common.Booking.Contracts.User.Requests;
using Otus.Booking.Common.Booking.Contracts.User.Responses;

namespace Booking.Auth.Test.User
{
    public class DeleteUserTests:BaseTest
    {
        private DeleteUserConsumer Consumer { get; }

        public DeleteUserTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>());

            var userRepository = new UserRepository(DataContext);
            Consumer = new DeleteUserConsumer(userRepository);
        }

        [Test]
        public async Task DeleteFilialTest()
        {
            var user = Fixture.Build<Domain.Entities.User>().Create();
            await DataContext.Users.AddAsync(user);

            await DataContext.SaveChangesAsync();
            var testHarness = new InMemoryTestHarness();
            var consumerHarness = testHarness.Consumer(() => Consumer);

            await testHarness.Start();

            var request = Fixture.Create<DeleteUser>();
            request.Id = user.Id;

            // Act
            await testHarness.InputQueueSendEndpoint.Send(request);
            var result = testHarness.Published.Select<DeleteUserResult>().FirstOrDefault()?.Context.Message;

            Assert.Multiple(() =>
            {
                Assert.That(testHarness.Consumed.Select<DeleteUser>().Any(), Is.True);
                Assert.That(consumerHarness.Consumed.Select<DeleteUser>().Any(), Is.True);
            });

            await testHarness.Stop();

        }
    }
}
