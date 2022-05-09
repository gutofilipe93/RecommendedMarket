using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Domain.Services.Dtos
{
    public class RefleshTokenResponseDto
    {
        [JsonProperty(PropertyName = "id_token")]
        public string idToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string refreshToken { get; set; }
    }
}
