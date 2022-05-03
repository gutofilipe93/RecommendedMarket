using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Domain.Services.Dtos
{
    public class TokenResponse
    {
        public string idToken { get; set; }
        public string refreshToken { get; set; }
    }
}
