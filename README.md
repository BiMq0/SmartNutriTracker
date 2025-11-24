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

## Contribución Externa

1. Haz un fork del repositorio
2. Crea una rama (`git checkout -b feature/nueva-funcionalidad`)
3. Haz commit de tus cambios (`git commit -m 'Agrega nueva funcionalidad'`)
4. Haz push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request 

---
