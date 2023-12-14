using HandcashClient.Models;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;

namespace HandcashClient.Tests
{
    public class GetItemsByOrderTests
    {
        private readonly HandcashClient _client;

        public GetItemsByOrderTests()
        {
            _client = HandcashClientTestHelper.CreateHandcashClient();
        }

        [Fact]
        public async Task GetItemsByOrder_ShouldReturnItems()
        {
            // Arrange
            var orderId = "657a48f3bddf5fe0d52afc26"; // replace with your order id

            // Act
            var items = await _client.GetItemsByOrder(orderId);

            // Assert
            items.Should().BeOfType<List<Item>>();
            foreach (var item in items)
            {
                Console.WriteLine($"Item Origin: {item.origin}");
                Console.WriteLine($"Item Image URL: {item.imageUrl}");
                Console.WriteLine($"Item Name: {item.name}");
                foreach (var attribute in item.attributes)
                {
                    Console.WriteLine($"Attribute Name: {attribute.name}, Value: {attribute.value}");
                }
                Console.WriteLine("---");
            }
        }
    }
}
