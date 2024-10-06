using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sherad.Domain.DTOs
{
    public class DoctorDTO
    {
        [JsonPropertyName("LastName")]
        public string LastName { get; set; }
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }
    }
}
