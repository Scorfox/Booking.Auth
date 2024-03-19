using AutoFixture;
using AutoMapper;
using Booking.Auth.Application.Consumers.Company;
using Booking.Auth.Application.Mappings;
using Booking.Auth.Application.Repositories;
using Booking.Auth.Persistence.Repositories;
using MassTransit.Testing;
using Otus.Booking.Common.Booking.Contracts.Company.Requests;
using Otus.Booking.Common.Booking.Contracts.Company.Responses;

namespace Booking.Auth.Test.Company;

public class GetCompanyTest : BaseTest
{
    private GetCompanyConsumer Consumer { get; }
    private ICompanyRepository CompanyRepository { get; }

    public GetCompanyTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<CompanyMapper>());
        
        CompanyRepository = new CompanyRepository(DataContext);
        Consumer = new GetCompanyConsumer(CompanyRepository, new Mapper(config));
    }

    [Test]
    public async Task GetCompanyByIdTest_ReturnsSuccess()
    {
        // Arrange
        var company = Fixture.Build<Domain.Entities.Company>().Without(e => e.Filials).Create();
        await CompanyRepository.CreateAsync(company);

        var request = Fixture.Build<GetCompanyById>()
            .With(e => e.Id, company.Id)
            .Create();

        var testHarness = new InMemoryTestHarness();
        testHarness.Consumer(() => Consumer);
        await testHarness.Start();

        // Act
        await testHarness.InputQueueSendEndpoint.Send(request);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(testHarness.Consumed.Select<GetCompanyById>().Any(), Is.True);
            Assert.That(testHarness.Published.Select<GetCompanyResult>().Any(), Is.True);
        });

        await testHarness.Stop();
    }
}