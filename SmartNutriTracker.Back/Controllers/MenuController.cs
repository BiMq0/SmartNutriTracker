using Microsoft.AspNetCore.Mvc;
using SmartNutriTracker.Back.Services.Menus;
using SmartNutriTracker.Shared.DTOs.Menus;
using Microsoft.Extensions.Logging;

namespace SmartNutriTracker.Back.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuController> _logger; // Logger agregado

        public MenuController(IMenuService menuService, ILogger<MenuController> logger)
        {
            _menuService = menuService;
            _logger = logger;
        }

        // GET: api/Menu
        [HttpGet]
        public async Task<ActionResult<List<MenuDTO>>> GetAll()
        {
            _logger.LogInformation("Solicitud GET: Obtener todos los menús."); // LOG

            var menus = await _menuService.GetAllAsync();

            _logger.LogInformation("Total de menús obtenidos: {Cantidad}", menus.Count); // LOG

            return Ok(menus);
        }

        // GET: api/Menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDTO>> GetById(int id)
        {
            _logger.LogInformation("Solicitud GET: Obtener menú por ID: {Id}", id); // LOG

            var menu = await _menuService.GetByIdAsync(id);

            if (menu == null)
            {
                _logger.LogWarning("Menú no encontrado con ID: {Id}", id); // LOG
                return NotFound();
            }

            _logger.LogInformation("Menú encontrado con ID: {Id}", id); // LOG

            return Ok(menu);
        }

        // POST: api/Menu
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateMenuDTO dto)
        {
            _logger.LogInformation("Solicitud POST: Crear menú para fecha: {Fecha}", dto.Fecha); // LOG

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación fallida al crear menú."); // LOG
                return BadRequest(ModelState);
            }

            var menuId = await _menuService.CreateAsync(dto);

            _logger.LogInformation("Menú creado exitosamente con ID: {Id}", menuId); // LOG

            return CreatedAtAction(nameof(GetById), new { id = menuId }, menuId);
        }

        // PUT: api/Menu/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateMenuDTO dto)
        {
            _logger.LogInformation("Solicitud PUT: Actualizar menú con ID: {Id}", id); // LOG

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validación fallida al actualizar menú con ID: {Id}", id); // LOG
                return BadRequest(ModelState);
            }

            var success = await _menuService.UpdateAsync(id, dto);

            if (!success)
            {
                _logger.LogWarning("No se pudo actualizar. Menú no encontrado con ID: {Id}", id); // LOG
                return NotFound();
            }

            _logger.LogInformation("Menú actualizado correctamente con ID: {Id}", id); // LOG

            return NoContent();
        }

        // DELETE: api/Menu/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Solicitud DELETE: Eliminar menú con ID: {Id}", id); // LOG

            var success = await _menuService.DeleteAsync(id);

            if (!success)
            {
                _logger.LogWarning("No se pudo eliminar. Menú no encontrado con ID: {Id}", id); // LOG
                return NotFound();
            }

            _logger.LogInformation("Menú eliminado exitosamente con ID: {Id}", id); // LOG

            return NoContent();
        }
    }
}
