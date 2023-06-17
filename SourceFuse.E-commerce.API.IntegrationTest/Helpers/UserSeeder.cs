using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SourceFuse.E_commerce.Common.Encript_Decrypt;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Bogus.DataSets.Name;

namespace SourceFuse.E_commerce.API.IntegrationTest.Helpers
{
    public class UserSeeder
    {
        public static async Task<string> AuthenticateUser(HttpClient httpClient, string email, string username, string password)
        {
            var request = new
            {
                Email = email,
                UserName= username,
                Password = password
            };

            var result = await httpClient.PostAsJsonAsync("api/User/Sign-in", request);
            var message = await result.Content.ReadAsStringAsync();
            var tokenModel = JsonConvert.DeserializeObject<TokenModel>(message);
            return tokenModel.Token;
        }
    }
}
