using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.Alimentos;

namespace SmartNutriTracker.Back.Services.Alimentos;

public class AlimentoService : IAlimentoService
{
    private readonly ApplicationDbContext _context;

    public AlimentoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AlimentoDTO>> GetAllAsync()
    {
        return await _context.Alimentos
            .Select(a => new AlimentoDTO
            {
                AlimentoId = a.AlimentoId,
                Nombre = a.Nombre,
                Calorias = a.Calorias,
                Proteinas = a.Proteinas,
                Carbohidratos = a.Carbohidratos,
                Grasas = a.Grasas
            }).ToListAsync();
    }

    public async Task<AlimentoDTO?> GetByIdAsync(int id)
    {
        var alimento = await _context.Alimentos.FindAsync(id);
        if (alimento == null) return null;

        return new AlimentoDTO
        {
            AlimentoId = alimento.AlimentoId,
            Nombre = alimento.Nombre,
            Calorias = alimento.Calorias,
            Proteinas = alimento.Proteinas,
            Carbohidratos = alimento.Carbohidratos,
            Grasas = alimento.Grasas
        };
    }

    public async Task<int> CreateAsync(CreateAlimentoDTO dto)
    {
        var entity = new Domain.Models.BaseModels.Alimento
        {
            Nombre = dto.Nombre,
            Calorias = dto.Calorias,
            Proteinas = dto.Proteinas,
            Carbohidratos = dto.Carbohidratos,
            Grasas = dto.Grasas
        };

        _context.Alimentos.Add(entity);
        await _context.SaveChangesAsync();
        return entity.AlimentoId;
    }

    public async Task<bool> UpdateAsync(int id, UpdateAlimentoDTO dto)
    {
        var alimento = await _context.Alimentos.FindAsync(id);
        if (alimento == null) return false;

        alimento.Nombre = dto.Nombre;
        alimento.Calorias = dto.Calorias;
        alimento.Proteinas = dto.Proteinas;
        alimento.Carbohidratos = dto.Carbohidratos;
        alimento.Grasas = dto.Grasas;

        _context.Alimentos.Update(alimento);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var alimento = await _context.Alimentos.FindAsync(id);
        if (alimento == null) return false;

        _context.Alimentos.Remove(alimento);
        return await _context.SaveChangesAsync() > 0;
    }
}
