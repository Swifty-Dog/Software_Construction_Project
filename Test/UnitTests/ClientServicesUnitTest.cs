using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tests.Utilities;

public class ClientServicesUnitTests
{
    private readonly Mock<MyContext> _mockContext;
    private readonly Mock<DbSet<Client>> _mockDbSet;
    private readonly ClientServices _clientServices;

    public ClientServicesUnitTests()
    {
        // Create DbContextOptions for the mocked context
        var options = new DbContextOptionsBuilder<MyContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // In-memory database for isolated tests
            .Options;

        // Initialize the mocked DbSet and context
        _mockDbSet = new Mock<DbSet<Client>>();
        _mockContext = new Mock<MyContext>(options) { CallBase = true }; // Pass options to the constructor
        // _service = new ClientServices(_mockContext.Object);

        var mockData = new List<Client>
        {
            new Client
            {
                Id = 1,
                Name = "Raymond Inc",
                Address = "1296 Daniel Road Apt. 349",
                City = "Pierceview",
                Zip = "28301",
                Province = "Colorado",
                Country = "United States",
                ContactName = "Bryan Clark",
                ContactPhone = "242.732.3483x2573",
                ContactEmail = "robertcharles@example.net",
                CreatedAt = new DateTime(2010, 4, 28),
                UpdatedAt = new DateTime(2022, 2, 9)
            },
            new Client
            {
                Id = 2,
                Name = "Tech Solutions",
                Address = "45 Maple Street",
                City = "TechCity",
                Zip = "45678",
                Province = "California",
                Country = "United States",
                ContactName = "Sarah Jones",
                ContactPhone = "555-1234",
                ContactEmail = "sarah@example.com",
                CreatedAt = new DateTime(2015, 7, 19),
                UpdatedAt = new DateTime(2023, 1, 1)
            }
        };

        // Use DbSetMockExtensions to mock DbSet
        _mockDbSet.ReturnsDbSet(mockData);
        _mockContext.Setup(c => c.Client).Returns(_mockDbSet.Object);

        // Create ClientServices instance
        _clientServices = new ClientServices(_mockContext.Object);
    }

    [Fact]
    public async Task TestGetAllClients()
    {
        // Act
        var clients = await _clientServices.GetClients();

        // Assert
        Xunit.Assert.NotNull(clients);
        Xunit.Assert.Equal(2, clients.Count());
    }

    [Fact]
    public async Task TestGetClientById()
    {
        // Act
        var client = await _clientServices.GetClientById(1);

        // Assert
        Xunit.Assert.NotNull(client);
        Xunit.Assert.Equal(1, client.Id);
        Xunit.Assert.Equal("Raymond Inc", client.Name);
    }

    [Fact]
    public async Task TestGetNonExistentClientById()
    {
        // Act
        var client = await _clientServices.GetClientById(99);

        // Assert
        Xunit.Assert.Null(client);
    }

    [Fact]
    public async Task TestAddClient()
    {
        // Arrange
        var newClient = new Client
        {
            Id = 3,
            Name = "New Company",
            Address = "100 Innovation Drive",
            City = "InnovateCity",
            Zip = "99999",
            Province = "Nevada",
            Country = "United States",
            ContactName = "John Doe",
            ContactPhone = "123-456-7890",
            ContactEmail = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var addedClient = await _clientServices.AddClient(newClient);

        // Assert
        Xunit.Assert.NotNull(addedClient);
        Xunit.Assert.Equal(newClient.Id, addedClient.Id);
        _mockDbSet.Verify(d => d.Add(It.IsAny<Client>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task TestAddExistingClient()
    {
        // Arrange
        var existingClient = new Client
        {
            Id = 1,
            Name = "Raymond Inc",
            Address = "1296 Daniel Road Apt. 349",
            City = "Pierceview",
            Zip = "28301",
            Province = "Colorado",
            Country = "United States",
            ContactName = "Bryan Clark",
            ContactPhone = "242.732.3483x2573",
            ContactEmail = "robertcharles@example.net",
            CreatedAt = new DateTime(2010, 4, 28),
            UpdatedAt = new DateTime(2022, 2, 9)
        };

        // Act
        var addedClient = await _clientServices.AddClient(existingClient);

        // Assert
        Xunit.Assert.Null(addedClient);
        _mockDbSet.Verify(d => d.Add(It.IsAny<Client>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task TestUpdateClient()
    {
        // Arrange
        var updatedClient = new Client
        {
            Id = 1,
            Name = "Updated Name",
            Address = "Updated Address",
            City = "Updated City",
            Zip = "Updated Zip",
            Province = "Updated Province",
            Country = "Updated Country",
            ContactName = "Updated Contact",
            ContactPhone = "Updated Phone",
            ContactEmail = "updated@example.com",
            CreatedAt = new DateTime(2010, 4, 28),
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _clientServices.UpdateClient(1, updatedClient);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Updated Name", result.Name);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task TestUpdateNonExistentClient()
    {
        // Arrange
        var updatedClient = new Client
        {
            Id = 99,
            Name = "Nonexistent",
            Address = "N/A",
            City = "N/A",
            Zip = "N/A",
            Province = "N/A",
            Country = "N/A",
            ContactName = "N/A",
            ContactPhone = "N/A",
            ContactEmail = "N/A@example.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var result = await _clientServices.UpdateClient(99, updatedClient);

        // Assert
        Xunit.Assert.Null(result);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task TestDeleteClient()
    {
        // Act
        var result = await _clientServices.DeleteClient(1);

        // Assert
        Xunit.Assert.True(result);
        _mockDbSet.Verify(d => d.Remove(It.IsAny<Client>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task TestDeleteNonExistentClient()
    {
        // Act
        var result = await _clientServices.DeleteClient(99);

        // Assert
        Xunit.Assert.False(result);
        _mockDbSet.Verify(d => d.Remove(It.IsAny<Client>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
    }
}
