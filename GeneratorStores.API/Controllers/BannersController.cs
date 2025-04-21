using GeneratorStores.DataAccess.Entities;
using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BannersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var banners = await _unitOfWork.Banners.GetAllAsync();
            return Ok(banners);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var banner = await _unitOfWork.Banners.GetByIdAsync(id);
            if (banner == null) return NotFound();
            return Ok(banner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Banner banner)
        {
            await _unitOfWork.Banners.AddAsync(banner);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(Get), new { id = banner.Id }, banner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Banner banner)
        {
            if (id != banner.Id) return BadRequest("ID mismatch");
            await _unitOfWork.Banners.UpdateAsync(banner);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _unitOfWork.Banners.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
