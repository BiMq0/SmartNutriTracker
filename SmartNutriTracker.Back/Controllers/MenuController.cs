using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Menus;
using SmartNutriTracker.Shared.DTOs.Menus;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // GET: api/Menu
        [HttpGet]
        public async Task<ActionResult<List<MenuDTO>>> GetAll()
        {
            var menus = await _menuService.GetAllAsync();
            return Ok(menus);
        }

        // GET: api/Menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDTO>> GetById(int id)
        {
            var menu = await _menuService.GetByIdAsync(id);
            if (menu == null)
                return NotFound();
            return Ok(menu);
        }

        // POST: api/Menu
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateMenuDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Validaciones

            var menuId = await _menuService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = menuId }, menuId);
        }

        // PUT: api/Menu/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateMenuDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Validaciones

            var success = await _menuService.UpdateAsync(id, dto);
            if (!success)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/Menu/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var success = await _menuService.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
