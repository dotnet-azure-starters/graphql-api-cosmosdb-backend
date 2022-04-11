using System.Text.Json.Serialization;

namespace GraphQlCosmosDbStarter.Data.Models
{
    public class Site : BaseEntity
    {
        [JsonPropertyName("siteNumber")]
        public string SiteNumber { get; set; } = null!;

        [JsonPropertyName("siteName")]
        public string SiteName { get; set; } = null!;

        [JsonPropertyName("siteAddress")]
        public string SiteAddress { get; set; } = null!;

        public string PartitionKey => SiteNumber;
    }
}