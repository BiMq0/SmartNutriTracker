using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.Menus;
using SmartNutriTracker.Shared.DTOs.Alimentos;
using SmartNutriTracker.Domain.Models.BaseModels; // <- Agregado

namespace SmartNutriTracker.Back.Services.Menus;

public class MenuService : IMenuService
{
    private readonly ApplicationDbContext _context;

    public MenuService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuDTO>> GetAllAsync()
    {
        return await _context.Menus
            .Include(m => m.MenuAlimentos) // Incluir tabla de unión
                .ThenInclude(ma => ma.Alimento) // Incluir los alimentos
            .Select(m => new MenuDTO
            {
                MenuId = m.MenuId,
                Fecha = m.Fecha,
                Alimentos = m.MenuAlimentos
                    .Select(ma => new AlimentoDTO
                    {
                        AlimentoId = ma.Alimento.AlimentoId,
                        Nombre = ma.Alimento.Nombre,
                        Calorias = ma.Alimento.Calorias,
                        Proteinas = ma.Alimento.Proteinas,
                        Carbohidratos = ma.Alimento.Carbohidratos,
                        Grasas = ma.Alimento.Grasas
                    }).ToList()
            })
            .ToListAsync();
    }

    public async Task<MenuDTO?> GetByIdAsync(int id)
    {
        var menu = await _context.Menus
            .Include(m => m.MenuAlimentos)
                .ThenInclude(ma => ma.Alimento)
            .FirstOrDefaultAsync(m => m.MenuId == id);

        if (menu == null)
            return null;

        return new MenuDTO
        {
            MenuId = menu.MenuId,
            Fecha = menu.Fecha,
            Alimentos = menu.MenuAlimentos
                .Select(ma => new AlimentoDTO
                {
                    AlimentoId = ma.Alimento.AlimentoId,
                    Nombre = ma.Alimento.Nombre,
                    Calorias = ma.Alimento.Calorias,
                    Proteinas = ma.Alimento.Proteinas,
                    Carbohidratos = ma.Alimento.Carbohidratos,
                    Grasas = ma.Alimento.Grasas
                }).ToList()
        };
    }

    public async Task<int> CreateAsync(CreateMenuDTO dto)
    {
        var menu = new Menu // <- Aquí se cambió
        {
            Fecha = dto.Fecha
        };

        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
        return menu.MenuId;
    }

    public async Task<bool> UpdateAsync(int id, UpdateMenuDTO dto)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu == null)
            return false;

        menu.Fecha = dto.Fecha;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu == null)
            return false;

        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();
        return true;
    }
}
