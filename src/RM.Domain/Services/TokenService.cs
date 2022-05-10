using Newtonsoft.Json;
using RestSharp;
using RM.Domain.Interfaces.Services;
using RM.Domain.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RM.Domain.Services
{
    public class TokenService : ITokenService
    {
        public async Task<dynamic> GetTokenAsync(string user, string password)
        {
            var client = new RestClient("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyDQ2SuHaluD0m2m-HzILWqEBTE_LPCf6Tc");
            var request = new RestRequest();
            request.Method = Method.POST;
            var obj = new
            {
                email = user,
                password,
                returnSecureToken = true
            };

            var json = JsonConvert.SerializeObject(obj);
            request.AddJsonBody(obj);
            var response = await client.ExecuteAsync(request);
            var result = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            return result;
        }

        public async Task<dynamic> RefleshTokenAsync(string refreshToken)
        {
            var client = new RestClient("https://securetoken.googleapis.com/v1/token?key=AIzaSyDQ2SuHaluD0m2m-HzILWqEBTE_LPCf6Tc");
            var request = new RestRequest();
            request.Method = Method.POST;
            var obj = new
            {
                refresh_token = refreshToken,
                grant_type = "refresh_token"
            };

            var json = JsonConvert.SerializeObject(obj);
            request.AddJsonBody(obj);
            var response = await client.ExecuteAsync(request);
            var result = JsonConvert.DeserializeObject<RefleshTokenResponseDto>(response.Content);
            return result;
        }
    }
}
