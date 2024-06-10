using Newtonsoft.Json;

namespace Razor.Models { 
public class ProductViewModel
{
    [JsonProperty("idProduct")]
    public int Id { get; set; }

    [JsonProperty("productName")]
    public string Name { get; set; }

    [JsonProperty("subtitle")]
    public string Subtitle { get; set; }

    [JsonProperty("amountOf")]
    public int AmountOf { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("userId")]
    public int UserId { get; set; }

    [JsonProperty("categoryIds")]
    public IdList CategoryIds { get; set; }

    [JsonProperty("photos")]
    public PhotoList Photos { get; set; }
}

public class IdList
{
    [JsonProperty("$values")]
    public List<int> Values { get; set; }
}

public class PhotoList
{
    [JsonProperty("$values")]
    public List<string> Values { get; set; }
} }