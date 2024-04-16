using System.Net;
using Curso.Api.Endpoints.Clients;
using Curso.Business.Features.Clients.Queries.GetClientById;
using FastEndpoints;
using FastEndpoints.Testing;
using Microsoft.EntityFrameworkCore;

namespace Curso.Api.IntegrationTests.Endpoints.Clients;

public class GetClientByIdTests(IntegrationTestsFixture fixture) : TestBase<IntegrationTestsFixture>
{
    [Fact]
    public async Task Given_ExistingClientId_Should_ReturnClient()
    {
        var client = await fixture.GetDbContext().Clients.FirstAsync();
        var expectedResponse = new GetClientByIdResponse
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            BirthDate = client.BirthDate
        };

        var (httpResponse, response) = await fixture.Client.GETAsync<
            GetClientByIdEndpoint,
            GetClientByIdRequest,
            GetClientByIdResponse
        >(new GetClientByIdRequest { Id = client.Id });

        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(expectedResponse, response);
    }

    [Theory]
    [InlineData("4777e101-2f61-51b7-b59f-9e6129322e54")]
    [InlineData("1340ce0d-0c87-57de-8bd8-afed0609d369")]
    [InlineData("29bd3930-6205-5bc9-9566-38f0f6c1a8b0")]
    public async Task Given_NonExistingClientId_Should_ReturnNotFound(string id)
    {
        var (httpResponse, response) = await fixture.Client.GETAsync<
            GetClientByIdEndpoint,
            GetClientByIdRequest,
            GetClientByIdResponse
        >(new GetClientByIdRequest { Id = Guid.Parse(id) });

        Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
    }
}
