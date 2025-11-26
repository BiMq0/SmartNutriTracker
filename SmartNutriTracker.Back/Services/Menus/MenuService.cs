using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.Menus;

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
            .Include(m => m.MenuAlimentos)
            .ThenInclude(ma => ma.Alimento)
            .Select(m => new MenuDTO
            {
                MenuId = m.MenuId,
                Fecha = m.Fecha,
                Alimentos = m.MenuAlimentos.Select(ma => new MenuAlimentoDTO
                {
                    MenuAlimentoId = ma.MenuAlimentoId,
                    AlimentoId = ma.AlimentoId,
                    NombreAlimento = ma.Alimento!.Nombre,
                    Calorias = ma.Alimento.Calorias
                }).ToList()
            }).ToListAsync();
    }

    public async Task<MenuDTO?> GetByIdAsync(int id)
    {
        var menu = await _context.Menus
            .Include(m => m.MenuAlimentos)
            .ThenInclude(ma => ma.Alimento)
            .FirstOrDefaultAsync(m => m.MenuId == id);

        if (menu == null) return null;

        return new MenuDTO
        {
            MenuId = menu.MenuId,
            Fecha = menu.Fecha,
            Alimentos = menu.MenuAlimentos.Select(ma => new MenuAlimentoDTO
            {
                MenuAlimentoId = ma.MenuAlimentoId,
                AlimentoId = ma.AlimentoId,
                NombreAlimento = ma.Alimento!.Nombre,
                Calorias = ma.Alimento.Calorias
            }).ToList()
        };
    }

    public async Task<int> CreateAsync(CreateMenuDTO dto)
    {
        var menu = new Domain.Models.BaseModels.Menu
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
        if (menu == null) return false;

        menu.Fecha = dto.Fecha;
        _context.Menus.Update(menu);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu == null) return false;

        _context.Menus.Remove(menu);
        return await _context.SaveChangesAsync() > 0;
    }
}
