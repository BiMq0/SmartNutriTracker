using Microsoft.EntityFrameworkCore;
using SmartNutriTracker.Back.Database;
using SmartNutriTracker.Shared.DTOs.Alimentos;
using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Back.Services.Alimentos
{
    public class AlimentoService : IAlimentoService
    {
        private readonly ApplicationDbContext _db;

        public AlimentoService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<AlimentoDTO> CreateAsync(CreateAlimentoDTO dto)
        {
            var entity = new Alimento
            {
                Nombre = dto.Nombre,
                Calorias = dto.Calorias,
                Proteinas = dto.Proteinas,
                Carbohidratos = dto.Carbohidratos,
                Grasas = dto.Grasas
            };

            _db.Alimentos.Add(entity);
            await _db.SaveChangesAsync();

            return new AlimentoDTO
            {
                AlimentoId = entity.AlimentoId,
                Nombre = entity.Nombre,
                Calorias = entity.Calorias,
                Proteinas = entity.Proteinas,
                Carbohidratos = entity.Carbohidratos,
                Grasas = entity.Grasas
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Alimentos.FindAsync(id);
            if (entity == null) return false;

            _db.Alimentos.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AlimentoDTO>> GetAllAsync()
        {
            return await _db.Alimentos
                .Select(a => new AlimentoDTO
                {
                    AlimentoId = a.AlimentoId,
                    Nombre = a.Nombre,
                    Calorias = a.Calorias,
                    Proteinas = a.Proteinas,
                    Carbohidratos = a.Carbohidratos,
                    Grasas = a.Grasas
                })
                .ToListAsync();
        }

        public async Task<AlimentoDTO?> GetByIdAsync(int id)
        {
            var a = await _db.Alimentos.FindAsync(id);
            if (a == null) return null;

            return new AlimentoDTO
            {
                AlimentoId = a.AlimentoId,
                Nombre = a.Nombre,
                Calorias = a.Calorias,
                Proteinas = a.Proteinas,
                Carbohidratos = a.Carbohidratos,
                Grasas = a.Grasas
            };
        }

        public async Task<AlimentoDTO?> UpdateAsync(UpdateAlimentoDTO dto)
        {
            var entity = await _db.Alimentos.FindAsync(dto.AlimentoId);
            if (entity == null) return null;

            entity.Nombre = dto.Nombre;
            entity.Calorias = dto.Calorias;
            entity.Proteinas = dto.Proteinas;
            entity.Carbohidratos = dto.Carbohidratos;
            entity.Grasas = dto.Grasas;

            await _db.SaveChangesAsync(); // ✅ Cambios guardados en la BD

            return new AlimentoDTO
            {
                AlimentoId = entity.AlimentoId,
                Nombre = entity.Nombre,
                Calorias = entity.Calorias,
                Proteinas = entity.Proteinas,
                Carbohidratos = entity.Carbohidratos,
                Grasas = entity.Grasas
            };
        }
    }
}
