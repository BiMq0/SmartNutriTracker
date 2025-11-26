using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartNutriTracker.Back.Handlers;
using SmartNutriTracker.Domain.Models.BaseModels;
using SmartNutriTracker.Shared; 

namespace SmartNutriTracker.Back.Services.Security
{
    public interface IJwtTokenService
    {
        string GenerateToken(Usuario usuario, int? expireMinutes = null);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JWTSettings _settings;
        public JwtTokenService(IOptions<JWTSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(Usuario usuario, int? expireMinutes = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? usuario.RolId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expireMinutes ?? 60);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}