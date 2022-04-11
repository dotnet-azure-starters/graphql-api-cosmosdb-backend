using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GraphQlCosmosDbStarter.Data.Models
{
    public class BaseEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;
    }
}
