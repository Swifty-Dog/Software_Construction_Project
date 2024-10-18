// using System.Net;
// using System.Net.Http;
// using NUnit.Framework;

// [TestFixture]
// public class WarehouseControllerTests
// {
//     [Test]
//     public void HttpStatusCodeTest()
//     {
//         // Arrange: Create an instance of HttpClient
//         using var httpClient = new HttpClient();

//         // Act: Send a GET request to the endpoint (ensure localhost is running the API)
//         HttpResponseMessage response = httpClient.GetAsync("http://localhost:5000/api/v1/Warehouses").Result;

//         // Assert: Check that the response status code is 200 OK
//         NUnit.Framework.Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//     }
// }
