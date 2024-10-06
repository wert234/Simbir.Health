using Sherad.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sherad.Domain.DTOs
{
    public class HospitalDTO
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("Address")]
        public string Address { get; set; }
        [JsonPropertyName("ContactPhone")]
        public string ContactPhone { get; set; }
    }
}
