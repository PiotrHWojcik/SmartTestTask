using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartTestTask;
using SmartTestTask.Models.DTOs;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace SmartTestTask.Tests
{


    public class TestTaskCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public TestTaskCheckTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateAndRetrieveContracts()
        {
            // Arrange
            var apiKey = "your-static-randomly-generated-key";
            _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);

            var createDto = new CreateEquipmentPlacementContractDto
            {
                ProductionFacilityCode = "Facility1",
                ProcessEquipmentCode = "Equipment1",
                EquipmentQuantity = 5
            };

            var content = new StringContent(JsonConvert.SerializeObject(createDto), Encoding.UTF8, "application/json");

            // Act - Create a new contract
            var createResponse = await _client.PostAsync("/api/EquipmentPlacementContracts", content);
            createResponse.EnsureSuccessStatusCode();

            // Act - Retrieve the list of contracts
            var retrieveResponse = await _client.GetAsync("/api/EquipmentPlacementContracts");
            retrieveResponse.EnsureSuccessStatusCode();

            var responseString = await retrieveResponse.Content.ReadAsStringAsync();
            var contracts = JsonConvert.DeserializeObject<List<EquipmentPlacementContractDto>>(responseString);

            // Assert
            Assert.NotEmpty(contracts);
            Assert.Contains(contracts, c => c.ProductionFacilityName == "Facility1" && c.ProcessEquipmentName == "Equipment1" && c.EquipmentQuantity == 5);
        }
    }

}