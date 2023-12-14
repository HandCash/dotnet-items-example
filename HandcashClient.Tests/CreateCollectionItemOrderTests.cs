using HandcashClient.Models;
using Moq;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Microsoft.Extensions.Configuration;


namespace HandcashClient.Tests
{
    public class CreateCollectionItemOrderTests
    {
        private readonly HandcashClient _client;

        private static List<CreateItemMetadata> GetTestItems()
        {
            return new List<CreateItemMetadata>
            {
                new CreateItemMetadata
                {
                    user = "612cba70e108780b4f6817ad", //optional
                    name = "Rafa",
                    rarity = "Mythic",
                    attributes = new List<ItemAttributeMetadata>
                    {
                        new ItemAttributeMetadata { name = "Edition", value = "Test", displayType = "string" },
                        new ItemAttributeMetadata { name = "Generation", value = "1", displayType = "number" },
                        new ItemAttributeMetadata { name = "Country", value = "Spain", displayType = "string" }
                    },
                    mediaDetails = new MediaDetails
                    {
                        image = new MediaFile
                        {
                            url = "https://res.cloudinary.com/handcash-iae/image/upload/v1702398977/items/jyn2qqyqyepqhqi9p661.webp",
                            imageHighResUrl = "https://res.cloudinary.com/handcash-iae/image/upload/v1697465892/items/zq0lupxoj8id1uedgz2h.png",
                            contentType = "image/webp"
                        }
                    },
                    quantity = 3
                },
            };
        }

        public CreateCollectionItemOrderTests()
        {
            _client = HandcashClientTestHelper.CreateHandcashClient();
        }

        [Fact]
        public async Task CreateCollectionItemOrder_ShouldReturnCreateItemsOrderWithValidId()
        {
            // Arrange
            var testItems = GetTestItems();
    
            // Act
            var result = await _client.CreateCollectionItemOrder("657a2e0b2acbecc1094d8227", testItems);

            // Assert
            result.Should().BeOfType<CreateItemsOrder>();
            result.id.Should().NotBeNullOrEmpty();
            Console.WriteLine($"Item Creation Order ID: {result.id}");
        }
    }
}
