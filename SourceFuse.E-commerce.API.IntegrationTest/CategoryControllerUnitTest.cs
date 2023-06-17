using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SourceFuse.E_commerce.API.IntegrationTest.Factory;
using SourceFuse.E_commerce.API.IntegrationTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SourceFuse.E_commerce.API.IntegrationTest
{
    public class CategoryControllerUnitTest : IClassFixture<InMemoryWebApplicationFactory<Startup>>
    {
        private readonly InMemoryWebApplicationFactory<Startup> _factory;
        private readonly string baseEndpoint;

        public CategoryControllerUnitTest(InMemoryWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            baseEndpoint = "api/Category";
        }

        [Fact]
        public void GetCategoryById_Test()
        {

        }

        [Fact]
        public void GetCategory_Should_Return_Unauthorized_For_No_Auth_Header_Test()
        {
            //arrange
            var httpClient = _factory.CreateClient();
            var expected = HttpStatusCode.Unauthorized;

            //act
            var httpResponse = httpClient.GetAsync(baseEndpoint).Result;

            // assert
            Assert.Equal(expected, httpResponse.StatusCode);
        }

        [Fact]
        public void GetCategory_Test()
        {
            //arrange
            var expected = HttpStatusCode.OK;
            var httpClient = _factory.CreateClient();

            //var token = UserSeeder.AuthenticateUser(httpClient, "test@gmail.com", "John.Smith", "1qaz@WSX").Result;
            var token = UserSeeder.AuthenticateUser(httpClient, "Alvena_Kemmer@hotmail.com", "Chasity.Davis", "1qaz@WSX").Result;

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //act
            var response = httpClient.GetAsync(baseEndpoint).Result;

            // assert
            Assert.Equal(expected, response.StatusCode);
        }

        [Fact]
        public void AddCategory_Test()
        {

        }


        [Fact]
        public void UpdateCategory_Test()
        {

        }

        [Fact]
        public void DeleteCategory_Test()
        {

        }
    }
}
