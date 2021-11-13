using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnrollmentAPI.Models
{
   public  class enrollmentModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "enrollmentchannel")]
        public string EnrollmentChannel { get; set; }

        [JsonProperty(PropertyName = "firstname")]

        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastname")]

        public string LastName { get; set; }
        [JsonProperty(PropertyName = "email")]

        public string Email { get; set; }
        [JsonProperty(PropertyName = "addressline")]

        public string addressline { get; set; }
        [JsonProperty(PropertyName = "postcode")]

        public string Postcode { get; set; }
        [JsonProperty(PropertyName = "membernum")]

        public string MemberNum { get; set; }
    }
}
