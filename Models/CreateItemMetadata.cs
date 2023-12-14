namespace HandcashClient.Models

{
    public class CreateItemMetadata
    {
        public string name { get; set; }
        public string? user { get; set; }
        public string? description { get; set; }
        public string? rarity { get; set; }
        public int quantity { get; set; } = 1;
        public List<ItemAttributeMetadata> attributes { get; set; }
        public MediaDetails mediaDetails { get; set; }
        public List<Royalty>? royalties { get; set; }

        public CreateItemMetadata()
        {
            attributes = new List<ItemAttributeMetadata>();
            mediaDetails = new MediaDetails();
        }
    }

    public class MediaFile
    {
        public string url { get; set; }
        public string contentType { get; set; }
        public string? imageHighResUrl { get; set; }
    }

    public class MediaDetails
    {
        public MediaFile image { get; set; }
        public MediaFile? multimedia { get; set; }

        public MediaDetails()
        {
            image = new MediaFile();
        }
    }
}
