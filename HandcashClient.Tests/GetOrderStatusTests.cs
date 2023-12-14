using HandcashClient.Models;
using Moq;
using FluentAssertions;

namespace HandcashClient.Tests
{
    public class GetOrderStatusTests
    {
        private readonly HandcashClient _client;

        public GetOrderStatusTests()
        {
            _client = HandcashClientTestHelper.CreateHandcashClient();
        }

        [Fact]
        public async Task GetOrderStatus_ShouldReturnCorrectOrderStatus()
        {
            // Arrange
            var orderId = "657a48f3bddf5fe0d52afc26";

            // Act
            var result = await _client.GetOrderStatus(orderId);

            // Assert
            result.Should().BeOfType<CreateItemsOrder>();
            result.id.Should().Be(orderId);
            result.status.Should().Be("completed");
            Console.WriteLine($"Order ID: {result.id}, Status: {result.status}");
        }
    }
}
