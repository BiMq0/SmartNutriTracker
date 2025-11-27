using System.Net.Http.Json;
using SmartNutriTracker.Shared.DTOs.Usuarios;

namespace SmartNutriTracker.Front.Services
{
    public class AuthService
    {
   private readonly HttpClient _httpClient;

  public AuthService(HttpClient httpClient)
        {
  _httpClient = httpClient;
      }

   public async Task<AuthResponse?> LoginAsync(LoginDTO loginDTO)
  {
      try
 {
       var response = await _httpClient.PostAsJsonAsync("api/user/AutenticarUsuario", loginDTO);
 
  if (response.IsSuccessStatusCode)
   {
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
   return result;
         }

   return null;
     }
          catch (Exception ex)
 {
     Console.WriteLine($"Error en login: {ex.Message}");
      return null;
          }
    }

  public async Task<bool> LogoutAsync()
        {
 try
  {
  var response = await _httpClient.PostAsync("api/user/CerrarSesion", null);
    return response.IsSuccessStatusCode;
       }
 catch (Exception ex)
     {
       Console.WriteLine($"Error al cerrar sesión: {ex.Message}");
      return false;
            }
  }

 public async Task<List<UsuarioRegistroDTO>> GetUsuariosAsync()
        {
  try
 {
   var response = await _httpClient.GetAsync("api/user/ObtenerUsuarios");
       
  if (response.IsSuccessStatusCode)
          {
 var usuarios = await response.Content.ReadFromJsonAsync<List<UsuarioRegistroDTO>>();
    return usuarios ?? new List<UsuarioRegistroDTO>();
    }

     return new List<UsuarioRegistroDTO>();
      }
  catch (Exception ex)
       {
Console.WriteLine($"Error al obtener usuarios: {ex.Message}");
             return new List<UsuarioRegistroDTO>();
  }
        }
    }

  public class AuthResponse
  {
        public string Mensaje { get; set; } = string.Empty;
      public UsuarioRegistroDTO? Usuario { get; set; }
  }
}
