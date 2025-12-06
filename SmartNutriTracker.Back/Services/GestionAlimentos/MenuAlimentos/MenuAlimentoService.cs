using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.MenuAlimentos;

namespace SmartNutriTracker.Back.Services.MenuAlimentos;

public class MenuAlimentoService : IMenuAlimentoService
{
    private readonly ApplicationDbContext _context;

    public MenuAlimentoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AssignFoodToMenuAsync(CreateMenuAlimentoDTO dto)
    {
        var exists = await _context.MenuAlimentos.AnyAsync(ma =>
            ma.MenuId == dto.MenuId && ma.AlimentoId == dto.AlimentoId);

        if (exists) return false;

        var menuAlimento = new Domain.Models.BaseModels.MenuAlimento
        {
            MenuId = dto.MenuId,
            AlimentoId = dto.AlimentoId
        };

        _context.MenuAlimentos.Add(menuAlimento);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveFoodFromMenuAsync(int menuAlimentoId)
    {
        var menuAlimento = await _context.MenuAlimentos.FindAsync(menuAlimentoId);
        if (menuAlimento == null) return false;

        _context.MenuAlimentos.Remove(menuAlimento);
        return await _context.SaveChangesAsync() > 0;
    }
}
