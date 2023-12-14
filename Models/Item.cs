namespace HandcashClient.Models;

public class Item
{
	public string origin { get; set; }

	public string imageUrl { get; set; }

	public string name { get; set; }
	
	public List<ItemAttributeMetadata> attributes { get; set; }
}