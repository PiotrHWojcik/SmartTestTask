using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using SmartTestTask.Controllers;
using SmartTestTask.Data;
using SmartTestTask.Models;
using SmartTestTask.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using k8s.KubeConfigModels;

namespace SmartTestTask.Tests.Controllers
{
    public class EquipmentPlacementContractsControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly EquipmentPlacementContractsController _controller;

        public EquipmentPlacementContractsControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new EquipmentPlacementContractsController(_context);
        }

        [Fact]
        public async Task GetEquipmentPlacementContracts_ReturnsOkResult_WithListOfContracts()
        {
            // Arrange
            var contracts = new List<EquipmentPlacementContract>
            {
                new EquipmentPlacementContract { Id = 1, EquipmentQuantity = 10, ProductionFacility = new ProductionFacility { Name = "Facility1" }, ProcessEquipment = new ProcessEquipment { Name = "Equipment1" } },
                new EquipmentPlacementContract { Id = 2, EquipmentQuantity = 20, ProductionFacility = new ProductionFacility { Name = "Facility2" }, ProcessEquipment = new ProcessEquipment { Name = "Equipment2" } }
            };

            await _context.EquipmentPlacementContracts.AddRangeAsync(contracts);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetEquipmentPlacementContracts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnContracts = Assert.IsType<List<EquipmentPlacementContractDto>>(okResult.Value);
            Assert.Equal(2, returnContracts.Count);
        }

        [Fact]
        public async Task PostEquipmentPlacementContract_ReturnsBadRequest_WhenInvalidData()
        {
            // Arrange
            var dto = new CreateEquipmentPlacementContractDto
            {
                ProductionFacilityCode = "InvalidCode",
                ProcessEquipmentCode = "InvalidCode",
                EquipmentQuantity = 10
            };

            // Act
            var result = await _controller.PostEquipmentPlacementContract(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostEquipmentPlacementContract_ReturnsCreatedAtAction_WhenValidData()
        {
            // Arrange
            var productionFacility = new ProductionFacility { Id = 1, Code = "Facility1", Name = "Facility1", StandardArea = 100 };
            var processEquipment = new ProcessEquipment { Id = 1, Code = "Equipment1", Name = "Equipment1", Area = 10 };
            var dto = new CreateEquipmentPlacementContractDto
            {
                ProductionFacilityCode = "Facility1",
                ProcessEquipmentCode = "Equipment1",
                EquipmentQuantity = 5
            };

            await _context.ProductionFacilities.AddAsync(productionFacility);
            await _context.ProcessEquipments.AddAsync(processEquipment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.PostEquipmentPlacementContract(dto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnContract = Assert.IsType<EquipmentPlacementContract>(createdAtActionResult.Value);
            Assert.Equal(dto.EquipmentQuantity, returnContract.EquipmentQuantity);
        }
    }
}
