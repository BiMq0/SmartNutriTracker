# Observaciones de Revisión - SmartNutriTracker

## Estado de Entregas

### Equipos que entregaron

- **Autenticación y seguridad** - Mandaron Excel e hicieron la PR por sí mismos
- **Gestión de alimentos** - Test con xUnit sin funcionalidad real
- **Notificaciones y testing** - Mandaron Excel
- **Perfil y Reportes** - No mandaron testing
- **Recomendaciones inteligentes** - No mandaron testing e hicieron la PR por sí mismos
- **Registro de hábitos** - Se mandó testing

---

## Módulos Mergeados

### Autenticación y Seguridad

**Recomendaciones:**

- Se recomienda botón de ver contraseña
- Token se guarda en variable `_token` de servicio, puede compartirse entre usuarios, **muy inseguro**
- No se validan contraseñas seguras
- Solo se debería poder registrar usuarios con roles superiores
- No se muestran logs al realizar acciones (acciones probadas: inicio de sesión, registro)

**No implementa sus opciones en NavMenu**

---

### Recomendaciones Inteligentes

**Problemas críticos:**

- No se recuperan los datos del estudiante desde la base de datos (ni siquiera desde ID hardcodeado)
- La página no realiza ninguna acción al presionar el botón
- Se produce un error en la consola del navegador: método `pedirRecomendacion()` no definido
- Las opciones de formulario no son claras, ejemplo → objetivo: "mantener" (¿mantener qué?)
- Backend para nada legible y con full chatsito

** No implementa sus opciones en NavMenu**

---

### Registro de Hábitos

**Observaciones:**

- Se debe colocar el ID del estudiante manualmente (corregir con servicio de sesiones), se entiende la falta de esa implementación
- No se usa la paleta de colores seleccionada, se tiene una genérica de Bootstrap
- Interfaz poco amigable, ejemplo → en lugar de "Horas de sueño" tener "¿Cuántas horas dormiste hoy?"
- La búsqueda por el historial debería implementar varios filtros más:
  - Por nombre
  - Por edad
  - Por rango de horas de sueño
  - Por fechas de registro, etc.

** No implementa sus opciones en NavMenu**

---

### Registro de Consumo de Alimentos

**Problemas encontrados:**

- Se debe colocar el ID del estudiante manualmente (corregir con servicio de sesiones), se entiende la falta de esa implementación
- La cantidad se puede colocar manualmente, no se valida un rango incoherente (60000 gramos de un alimento)
- Mismas observaciones que en registro de hábitos: mejorar interfaz y usar métodos propios de la sesión
- Se puede colocar una fecha posterior a la actual, incoherencia de datos, no debería poder seleccionarse
- Los controllers implementan directamente el `DbContext`, **no se respeta la arquitectura**

** Coordinar DTOs entre equipos de Registro de Hábitos y Gestión de Alimentos, se tiene un error grave de referencia con `AlimentoDTO`**

** No implementa sus opciones en NavMenu**

---

### Gestión de Alimentos

**Problemas arquitectónicos:**

- Se implementa directamente `DbContext` en controller, no se usa ningún servicio
- No se debería poder crear menús que no contengan alimentos
- Si se implementaran las recomendaciones, coordinar con equipo de recomendaciones inteligentes
- No se valida que el nombre del alimento tenga texto, admite valores numéricos
- En `/alimentos` se debería poder filtrar por:
  - Nombre
  - Cantidad o rango de calorías
  - Proteínas, etc.

** No implementa sus opciones en NavMenu**

---

### Registro de Estudiantes

**Problemas encontrados:**

- Corregir detalles visuales en formulario de registro de estudiante, se tiene `bg-primary` como estilo
- No se está utilizando ningún servicio en `EditarPerfil` y por ello se tiene un error, no se puede probar la página (ver configuración en README.md en GitHub)
- `VerPerfil` no funciona y no utiliza ningún servicio, error de componente destruido antes de cargar la página
- No se está colocando ningún servicio en dashboard para recuperar los datos y no se puede ver el perfil a detalle de ningún estudiante
- Se está utilizando tanto `DbContext` como servicio en backend, **normalizar y llevar todo a `EstudianteService`/`IEstudianteService`**
- No se están utilizando las rutas de `.Shared`, **no se está siguiendo la arquitectura**

** Implementa sus opciones en NavMenu**

---

### Notificaciones y Testing

**Observaciones:**

- Se realizó testing a su primera sección, no se hace uso de `ILogger` en consola
- Se debe agregar un ejemplo explícito en front para verificar notificaciones (aunque hardcodeadas)
- Coordinar con equipo de tokens el envío de ID por cookies

---

## Observaciones Críticas y Faltas

### Problemas Generales

1. **blabalx eliminó script en `App.razor`** que otorgaba interactividad a la aplicación
2. **Equipo de registro de hábitos y alimento:** Coordinar sobre cambios en DTO que causa ambigüedad → `AlimentoDTO`

### Arquitectura

- Varios equipos implementan directamente `DbContext` en controllers
- No se respeta la arquitectura de servicios
- No se utilizan correctamente las rutas de `.Shared`

### UI/UX

- Falta de paleta de colores consistente
- Interfaces poco amigables
- Varios módulos no implementan sus opciones en `NavMenu`

### Seguridad

- Token compartido entre usuarios (crítico)
- No validación de contraseñas seguras
- Uso de logs de auditoría (implementacion pendiente por el resto de modulos)

---

## Acciones Requeridas

### Prioridad Alta

1. Corregir problema de `AlimentoDTO` entre equipos
2. Implementar servicio de sesiones para manejo de usuarios (docentes, estudiantes, nutricionistas y administradores)
3. Corregir manejo de tokens en autenticación
4. Normalizar uso de servicios en controllers, quitar `DbContext`

### Prioridad Media

1. Implementar opciones en `NavMenu` para todos los módulos
2. Agregar validaciones de datos (rangos, fechas, tipos)
3. Mejorar interfaces de usuario
4. Implementar logs de auditoría

### Prioridad Baja

1. Mejorar testing en todos los módulos
2. Implementar filtros avanzados
3. Agregar botón de ver contraseña
4. Mejorar mensajes y UX en formularios
