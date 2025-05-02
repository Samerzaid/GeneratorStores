using GeneratorStores.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeneratorStores.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoryProducts()
        {
            var result = await _unitOfWork.CategoryProducts.GetAllAsync();
            return Ok(result);
        }
    }
}
