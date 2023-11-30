using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.DTOs
{
    public class CustomerDTO
    {
        [JsonProperty("contacts")]
        public ContactsDTO? Contacts { get; set; }
        [JsonProperty("address")]
        public AddressDTO? Address { get; set; }
    }
}
