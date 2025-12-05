# SmartNutriTracker

## Descripción
SmartNutriTracker es un proyecto académico orientado a resolver un problema real dentro del entorno educativo: el seguimiento y la gestión de la nutrición de los estudiantes.  

---

## Características principales
- **Gestión de estudiantes y perfiles nutricionales**  
- **CRUD de alimentos** con información nutricional completa  
- **Registro diario de comidas, agua y hábitos saludables**  
- **Recomendaciones inteligentes** basadas en IMC, calorías y objetivos personales  
- **Alertas y notificaciones** sobre hábitos no saludables  
- **Reportes y dashboards gráficos** para análisis semanal  
- **Seguridad con JWT y roles de usuario**  
- **Documentación interactiva con Swagger**

---

## Tecnologías utilizadas
- **Backend:** .NET Core 8
- **Frontend:** Blazor Pages
- **Base de datos:** PostgreSQL + Supabase + Entity Framework Core  
- **Seguridad:** Autenticación JWT, gestión de roles y logs de auditoría  
- **Gestión de proyecto:** GitHub, Notion + Kanban, CI/CD con GitHub Actions

---

## Objetivo

El objetivo de SmartNutriTracker es **optimizar la gestión nutricional escolar**, ofreciendo una plataforma confiable que permita registrar, monitorear y analizar hábitos alimenticios, generando recomendaciones y reportes que apoyen la educación nutricional y el bienestar de los estudiantes.

---

## Descripción de Proyectos
### **SmartNutriTracker.Front**

> Proyecto <code>blazor</code> encargado de toda la UI de la aplicación, desarrollado con Blazor Pages, se encarga de la interacción directa con el cliente, pudiendo unicamente hacer peticiones a *SmartNutriTracker.Back* sin incluir lógica de negocio ni acceso a datos, y usar entidades DTO de *SmartNutriTracker.Shared*.

---
### **SmartNutriTracker.Shared**

> Proyecto <code>classlib</code> compartido entre *SmartNutriTracker.Front* y *SmartNutriTracker.Back*, encargado de almacenar de manera desacoplada las clases DTO y los Endpoints de peticion y recepción de datos.

---
### **SmartNutriTracker.Back**

> Proyecto <code>web</code> API REST realizad con .NET Core (Api Controllers y Minimal APIs) encargado de gestionar las peticiones y validaciones, la lógica de negocio y el acceso a la base de datos de la aplicación, utilizando JWT para la autenticación y el manejo conjunto de roles y permisos.

---
### **SmartNutriTracker.Test**

> Proyecto <code>xunit</code> de testing para el aseguramiento de calidad del proyecto, realizado con xUnit, su ejecución se encuentra automatizada junto a GitHub Actions en cada *Push* y *Pull Request*.
---

## Ejecución del Proyecto
Para la clonación del proyecto de debe introducir el siguiente comando: 

``` bash
git clone https://github.com/BiMq0/SmartNutriTracker.bo
```

### Prerequisitos
- Tener instalado .NET 8 SDK

### Ejecución y levantamiento del proyecto
**Desde VSCode o Terminal/CLI**
Para levantar el proyecto se deben seguir las siguientes indicaciones: 
- Frontend
*Dentro de la carpeta SmartNutriTracker.Front*
``` bash
# Para ejecución
dotnet run

# Para ejecución con recarga activa en desarrollo
dotnet watch run
```

- Backend
*Dentro de la carpeta SmartNutriTracker.Back*
``` bash
# Para ejecución
dotnet run

# Para ejecución con recarga activa en desarrollo
dotnet watch run
```
*En caso de tener Cookies configuradas con Secure y SameSite, se debe levantar el servidor con https y dar confianza de certificados de desarrollo en frontend*
``` bash
# Para levantar servidor
dotnet run/watch run --launch-profile https

# En SmartNutriTracker.Front
dotnet dev-certs https --trust
```

- Tests
*Dentro de la carpeta SmartNutriTracker.Test*
``` bash
dotnet test
```

### Configuracion de Servicios en Front

Para poder crear servicios y que las peticiones al backend se hagan correctamente, se debe seguir el siguiente ejemplo.

``` csharp
public class [Nombre]Service
{
  private readonly HttpClient _http

  public [Nombre]Service(IHttpClientFactory http)
  {
    _http = http.CreateClient("ApiClient")
  }
}
```

### Flujo de trabajo recomendado

Para el desarrollo de features se recomienda el siguiente flujo, ejemplo para obtener una lista de usuarios desde base de datos

1. Analizar los datos requeridos y si serán de creación o recuperación de datos.

``` csharp
// Para este ejemplo

int Id
string Nombre 
string Rol
```
2. Crear DTO en .Shared con datos requeridos, y constructor sin parametros, esto es importante para la deserialización de datos al pasar por las peticiones.

``` csharp
public class UsuarioRegistroDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }

        public UsuarioRegistroDTO() { }
    }
```

si se deben recuperar datos, usar entidad base en un segundo constructor y enlazar datos

``` csharp
public class UsuarioRegistroDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }

        public UsuarioRegistroDTO() { }
        public UsuarioRegistroDTO(Usuario usuario)
        {
            Id = usuario.UsuarioId;
            Nombre = usuario.Username;
            Rol = usuario.Rol != null ? usuario.Rol.Nombre : string.Empty;
        }
    }
```

3. Crear ruta para endpoint y peticion en Shared, si es nuevo, también colocar ruta BASE para el Controller

``` csharp
 public static class UsuariosEndpoints
    {
        public const string BASE = "api/user/";
        public const string OBTENER_TODOS_USUARIOS = "ObtenerUsuarios";
    }
```

4. Crear endpoint desde backend para funcionalidad, usando la ruta declarada en .Shared, si es nuevo, siempre enlazar Interfaz de servicio y en [Route()] colocar ruta BASE
``` csharp
[ApiController]
[Route(UsuariosEndpoints.BASE)]
public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(UsuariosEndpoints.OBTENER_TODOS_USUARIOS)]
        public async Task<List<UsuarioRegistroDTO>> ObtenerUsuarios()
        {
            return await _userService.ObtenerUsuariosAsync();
        }
    }
```

5. Declarar método en Interfaz de servicio
``` csharp
public interface IUserService
{
    Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync();
}
```

6. Implementar método de Interfaz en su respectiva clase, siempre heredar la interfaz, y en el constructor aplicar el DbContext para el uso de la base de datos con EF Core, y retornar todos los registros necesarios usando el DTO, esto mapea automaticamente los campos y datos requeridos
``` csharp
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UsuarioRegistroDTO>> ObtenerUsuariosAsync()
    {
        var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        // Mapeo automatico con Select
        return usuarios.Select(u => new UsuarioRegistroDTO(u)).ToList();
    }
}
```

7. Implementar servicio desde Front, si es nuevo, inicializar HttpClient con IHttpClientFactory en constructor, usar rutas y DTO de .Shared
``` csharp
public class UserService
    {
        private readonly HttpClient _http;
        private const string BASE = UsuariosEndpoints.BASE;

        public UserService(IHttpClientFactory http)
        {
            // Usar siempre esta configuracion puesto que ya se definio en Program.cs
            _http = http.CreateClient("ApiClient");
        }

        public async Task<List<UsuarioRegistroDTO>>  ObtenerUsuarios()
        {
            try
            {
                var url = BASE + UsuariosEndpoints.OBTENER_TODOS_USUARIOS;
                var response = await _http.GetFromJsonAsync<List<UsuarioRegistroDTO>>(url);

                if (response.IsSuccessStatusCode)
                {
                    // Retorna la respuesta 
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new UsuarioRegistroResponseDTO { Mensaje = $"Error: {ex.Message}" };
            }
        }
  }
```
8. Inyectar servicio en páginas donde se requiera y usar métodos.

## Contribución Externa

1. Haz un fork del repositorio
2. Crea una rama (`git checkout -b feature/nueva-funcionalidad`)
3. Haz commit de tus cambios (`git commit -m 'Agrega nueva funcionalidad'`)
4. Haz push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request 

---
