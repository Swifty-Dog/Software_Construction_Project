using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ClientServicesUnitTests
{
    private readonly Mock<MyContext> _mockContext;
    private readonly ClientServices _service;

    public ClientServicesUnitTests()
    {
        // Initialize the mock context
        _mockContext = new Mock<MyContext>();
        _service = new ClientServices(_mockContext.Object);
    }

    [Fact]
    public async Task TestGetAllClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, Name = "Client1", Address = "Address1", City = "City1", Zip = "10001", Province = "Province1", Country = "Country1", ContactName = "Contact1", ContactPhone = "12345", ContactEmail = "email1@test.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new Client { Id = 2, Name = "Client2", Address = "Address2", City = "City2", Zip = "10002", Province = "Province2", Country = "Country2", ContactName = "Contact2", ContactPhone = "67890", ContactEmail = "email2@test.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        };

        var mockSet = DbSetMockHelper.CreateMockDbSet(clients);
        _mockContext.Setup(c => c.Client).Returns(mockSet.Object);

        // Act
        var result = await _service.GetClients();

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task TestGetClientByIdValid()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, Name = "Client1", Address = "Address1", City = "City1", Zip = "10001", Province = "Province1", Country = "Country1", ContactName = "Contact1", ContactPhone = "12345", ContactEmail = "email1@test.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        };

        var mockSet = DbSetMockHelper.CreateMockDbSet(clients);
        _mockContext.Setup(c => c.Client).Returns(mockSet.Object);

        // Act
        var result = await _service.GetClientById(1);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Client1", result.Name);
    }

    [Fact]
    public async Task TestPostClientValid()
    {
        // Arrange
        var clients = new List<Client>();
        var mockSet = DbSetMockHelper.CreateMockDbSet(clients);
        _mockContext.Setup(c => c.Client).Returns(mockSet.Object);

        var newClient = new Client
        {
            Id = 1, Name = "New Client", Address = "New Address", City = "New City", Zip = "10003", Province = "New Province", Country = "New Country", ContactName = "New Contact", ContactPhone = "54321", ContactEmail = "newemail@test.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now
        };

        // Act
        var result = await _service.AddClient(newClient);

        // Assert
        Xunit.Assert.NotNull(result);
        _mockContext.Verify(c => c.Client.Add(newClient), Times.Once());
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
    }

    [Fact]
    public async Task TestDeleteClientValid()
    {
        // Arrange
        var clients = new List<Client>
        {
            new Client { Id = 1, Name = "Client1", Address = "Address1", City = "City1", Zip = "10001", Province = "Province1", Country = "Country1", ContactName = "Contact1", ContactPhone = "12345", ContactEmail = "email1@test.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        };

        var mockSet = DbSetMockHelper.CreateMockDbSet(clients);
        _mockContext.Setup(c => c.Client).Returns(mockSet.Object);

        // Act
        var result = await _service.DeleteClient(1);

        // Assert
        Xunit.Assert.True(result);
        _mockContext.Verify(c => c.Client.Remove(It.IsAny<Client>()), Times.Once());
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once());
    }
}
