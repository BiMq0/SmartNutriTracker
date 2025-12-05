using SmartNutriTracker.Domain.Models.BaseModels;

namespace SmartNutriTracker.Back.Services.Tokens;

public interface ITokenService
{
    string GenerarToken(Usuario usuario);
}