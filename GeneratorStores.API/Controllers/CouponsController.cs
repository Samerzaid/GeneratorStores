using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CouponsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var coupons = await _unitOfWork.Coupons.GetAllAsync();
            return Ok(coupons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var coupon = await _unitOfWork.Coupons.GetByIdAsync(id);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var coupon = await _unitOfWork.Coupons.GetByCodeAsync(code);
            if (coupon == null) return NotFound();
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Coupon coupon)
        {
            await _unitOfWork.Coupons.AddAsync(coupon);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetById), new { id = coupon.Id }, coupon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Coupon coupon)
        {
            if (id != coupon.Id) return BadRequest("ID mismatch");

            await _unitOfWork.Coupons.UpdateAsync(coupon);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfWork.Coupons.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
