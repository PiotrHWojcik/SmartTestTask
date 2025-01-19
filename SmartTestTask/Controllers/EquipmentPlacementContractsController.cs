using Moq;
using Xunit;
using SmartTestTask.Controllers;
using SmartTestTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartTestTask.Data;
using SmartTestTask.Models.DTOs;


namespace SmartTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentPlacementContractsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentPlacementContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentPlacementContracts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentPlacementContractDto>>> GetEquipmentPlacementContracts()
        {
            var contracts = await _context.EquipmentPlacementContracts
                .Include(e => e.ProductionFacility)
                .Include(e => e.ProcessEquipment)
                .Select(e => new EquipmentPlacementContractDto
                {
                    ProductionFacilityName = e.ProductionFacility.Name,
                    ProcessEquipmentName = e.ProcessEquipment.Name,
                    EquipmentQuantity = e.EquipmentQuantity
                })
                .ToListAsync();

            return Ok(contracts);
        }

        // POST: api/EquipmentPlacementContracts
        [HttpPost]
        public async Task<ActionResult<EquipmentPlacementContract>> PostEquipmentPlacementContract(CreateEquipmentPlacementContractDto dto)
        {
            var productionFacility = await _context.ProductionFacilities
                .FirstOrDefaultAsync(pf => pf.Code == dto.ProductionFacilityCode);
            var processEquipment = await _context.ProcessEquipments
                .FirstOrDefaultAsync(pe => pe.Code == dto.ProcessEquipmentCode);

            if (productionFacility == null || processEquipment == null)
            {
                return BadRequest("Invalid Production Facility or Process Equipment.");
            }

            var totalAreaRequired = dto.EquipmentQuantity * processEquipment.Area;
            if (totalAreaRequired > productionFacility.StandardArea)
            {
                return BadRequest("Insufficient area in the production facility.");
            }

            var contract = new EquipmentPlacementContract
            {
                ProductionFacilityId = productionFacility.Id,
                ProcessEquipmentId = processEquipment.Id,
                EquipmentQuantity = dto.EquipmentQuantity
            };

            _context.EquipmentPlacementContracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEquipmentPlacementContracts), new { id = contract.Id }, contract);
        }
    }
}
