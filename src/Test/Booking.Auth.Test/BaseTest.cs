using AutoFixture;
using Booking.Auth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Auth.Test;

public abstract class BaseTest
{
    protected DataContext DataContext { get; }
    protected IFixture Fixture { get; }
    
    public BaseTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .Options;

        DataContext = new DataContext(options);
        Fixture = new Fixture();
    }
}