using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNutriTracker.Shared.Loggers
{
    public static class Loggers_Globales
    {
        public const string LOGGER_USUARIO_STATUS_500 = "Error, Error interno del servidor";

        // ============================================
        // MÓDULO: AUTENTICACIÓN Y SEGURIDAD
        // ============================================

        // MENSAJES DE AUTENTICACIÓN
        public const string LOG_USUARIO_LOGIN_EXITOSO = "Success, Usuario inició sesión exitosamente.  Token generado. ";
        public const string LOG_USUARIO_LOGIN_FALLIDO = "Error, Intento de inicio de sesión fallido. Credenciales inválidas.";
        public const string LOG_USUARIO_LOGOUT_EXITOSO = "Success, Usuario cerró sesión exitosamente. ";

        //EXEPCIONES DE VALIDACION - USUARIOS
        public const string VAL_USUARIO_ID_INVALIDO = "Error, Validación fallida: El ID de usuario no es válido o no existe";
        public const string VAL_USUARIO_USERNAME_VACIO = "Error, Validación fallida: El nombre de usuario no puede estar vacío";
        public const string VAL_USUARIO_USERNAME_LONGITUD = "Error, Validación fallida: El Username no cumple con la longitud requerida";
        public const string VAL_USUARIO_USERNAME_DUPLICADO = "Error, Validación fallida: El username ya existe en el sistema";
        public const string VAL_USUARIO_USERNAME_CARACTERES_INVALIDOS = "Error, Validación fallida: El username contiene caracteres no permitidos";
        public const string VAL_USUARIO_PASSWORD_VACIO = "Error, Validación fallida: La contraseña no puede estar vacía";
        public const string VAL_USUARIO_PASSWORD_LONGITUD = "Error, Validación fallida: La contraseña no cumple con la longitud mínima requerida";
        public const string VAL_USUARIO_PASSWORD_COMPLEJIDAD = "Error, Validación fallida: La contraseña no cumple con los requisitos de complejidad";
        public const string VAL_USUARIO_PASSWORD_HASH_INVALIDO = "Error, Validación fallida: El hash de contraseña no es válido o está corrupto";
        public const string VAL_USUARIO_ROLID_INVALIDO = "Error, Validación fallida: El RolId no es válido o no existe";
        public const string VAL_USUARIO_ROLID_VACIO = "Error, Validación fallida: El RolId es requerido y no puede ser cero";
        public const string VAL_USUARIO_OBJETO_NULO = "Error, Validación fallida: El objeto Usuario no puede ser nulo";
        public const string VAL_USUARIO_EN_USO = "Error, Validación fallida: El usuario no puede ser eliminado porque tiene registros asociados";

        //ROLES
        public const string VAL_ROL_ID_INVALIDO = "Error, Validación fallida: El ID de rol no es válido o no existe";
        public const string VAL_ROL_NOMBRE_VACIO = "Error, Validación fallida: El nombre del rol no puede estar vacío";
        public const string VAL_ROL_NOMBRE_LONGITUD = "Error, Validación fallida: El nombre del rol no cumple con la longitud requerida";
        public const string VAL_ROL_NOMBRE_DUPLICADO = "Error, Validación fallida: El rol ya existe en el sistema";
        public const string VAL_ROL_OBJETO_NULO = "Error, Validación fallida: El objeto Rol no puede ser nulo";
        public const string VAL_ROL_EN_USO = "Error, Validación fallida: El rol no puede ser eliminado porque tiene usuarios asociados";
        public const string VAL_ROL_NOMBRE_FORMATO = "Error, Validación fallida: El nombre del rol contiene caracteres no permitidos";

        // ============================================
        // MÓDULO: PERFIL Y REPORTES
        // ============================================

        //ESTUDIANTE - LOGS DE OPERACIONES
        public const string LOG_ESTUDIANTE_REGISTRO_INICIO = "Info, Iniciando proceso de registro de estudiante";
        public const string LOG_ESTUDIANTE_REGISTRO_DTO_RECIBIDO = "Info, DTO recibido para registro de estudiante";
        public const string LOG_ESTUDIANTE_REGISTRO_SERVICIO_LLAMADA = "Info, Ejecutando servicio de registro de estudiante";
        public const string LOG_ESTUDIANTE_REGISTRO_EXITOSO = "Success, Estudiante registrado exitosamente";
        public const string LOG_ESTUDIANTE_REGISTRO_ERROR = "Error, Error al registrar estudiante";

        public const string LOG_ESTUDIANTE_OBTENER_LISTA_INICIO = "Info, Obteniendo lista completa de estudiantes";
        public const string LOG_ESTUDIANTE_OBTENER_LISTA_EXITOSO = "Success, Lista de estudiantes obtenida exitosamente";
        public const string LOG_ESTUDIANTE_OBTENER_LISTA_ERROR = "Error, Error al obtener la lista de estudiantes";

        public const string LOG_ESTUDIANTE_OBTENER_POR_ID_INICIO = "Info, Buscando estudiante por ID";
        public const string LOG_ESTUDIANTE_OBTENER_POR_ID_ENCONTRADO = "Success, Estudiante encontrado";
        public const string LOG_ESTUDIANTE_OBTENER_POR_ID_NO_ENCONTRADO = "Warning, Estudiante no encontrado";
        public const string LOG_ESTUDIANTE_OBTENER_POR_ID_ERROR = "Error, Error al obtener estudiante por ID";

        public const string LOG_ESTUDIANTE_ACTUALIZAR_INICIO = "Info, Iniciando actualización de perfil de estudiante";
        public const string LOG_ESTUDIANTE_ACTUALIZAR_DTO_RECIBIDO = "Info, DTO recibido para actualización de estudiante";
        public const string LOG_ESTUDIANTE_ACTUALIZAR_SERVICIO_LLAMADA = "Info, Ejecutando servicio de actualización de estudiante";
        public const string LOG_ESTUDIANTE_ACTUALIZAR_EXITOSO = "Success, Perfil de estudiante actualizado correctamente";
        public const string LOG_ESTUDIANTE_ACTUALIZAR_NO_ENCONTRADO = "Warning, No se pudo actualizar.  Estudiante no encontrado";
        public const string LOG_ESTUDIANTE_ACTUALIZAR_ARGUMENTO_INVALIDO = "Error, ArgumentException durante actualización del estudiante";
        public const string LOG_ESTUDIANTE_ACTUALIZAR_ERROR = "Error, Error interno al actualizar perfil del estudiante";

        public const string LOG_ESTUDIANTE_PERFIL_FAKE_INICIO = "Info, Solicitando perfil simulado para estudiante";
        public const string LOG_ESTUDIANTE_PERFIL_FAKE_GENERADO = "Success, Perfil simulado generado correctamente";

        public const string LOG_ESTUDIANTE_DASHBOARD_INICIO = "Info, Generando dashboard para estudiantes";
        public const string LOG_ESTUDIANTE_DASHBOARD_EXITOSO = "Success, Dashboard generado exitosamente";
        public const string LOG_ESTUDIANTE_DASHBOARD_ERROR = "Error, Error al generar dashboard de estudiantes";

        //ESTUDIANTE - VALIDACIONES
        public const string VAL_ESTUDIANTE_ID_INVALIDO = "Error, Validación fallida: El ID de estudiante no es válido o no existe";
        public const string VAL_ESTUDIANTE_NOMBRE_VACIO = "Error, Validación fallida: El NombreCompleto del estudiante no puede estar vacío";
        public const string VAL_ESTUDIANTE_NOMBRE_LONGITUD = "Error, Validación fallida: El NombreCompleto no cumple con la longitud requerida";
        public const string VAL_ESTUDIANTE_EDAD_INVALIDA = "Error, Validación fallida: La edad ingresada no está dentro del rango permitido";
        public const string VAL_ESTUDIANTE_EDAD_NEGATIVA = "Error, Validación fallida: La edad no puede ser negativa o cero";
        public const string VAL_ESTUDIANTE_PESO_INVALIDO = "Error, Validación fallida: El peso no está dentro del rango permitido";
        public const string VAL_ESTUDIANTE_PESO_NEGATIVO = "Error, Validación fallida: El peso no puede ser negativo o cero";
        public const string VAL_ESTUDIANTE_PESO_PRECISION = "Error, Validación fallida: El peso excede la precisión decimal permitida";
        public const string VAL_ESTUDIANTE_ALTURA_INVALIDA = "Error, Validación fallida: La altura no está dentro del rango permitido";
        public const string VAL_ESTUDIANTE_ALTURA_NEGATIVA = "Error, Validación fallida: La altura no puede ser negativa o cero";
        public const string VAL_ESTUDIANTE_ALTURA_PRECISION = "Error, Validación fallida: La altura excede la precisión decimal permitida";
        public const string VAL_ESTUDIANTE_OBJETO_NULO = "Error, Validación fallida: El objeto Estudiante no puede ser nulo";
        public const string VAL_ESTUDIANTE_EN_USO = "Error, Validación fallida: El estudiante no puede ser eliminado porque tiene registros asociados";
        public const string VAL_ESTUDIANTE_IMC_INCONSISTENTE = "Warning, Validación fallida: El IMC calculado parece inconsistente con los datos proporcionados";

        // ============================================
        // MÓDULO: GESTIÓN DE ALIMENTOS
        // ============================================

        //ALIMENTO - LOGS DE OPERACIONES
        public const string LOG_ALIMENTO_OBTENER_LISTA_INICIO = "Info, Iniciando obtención de lista de alimentos";
        public const string LOG_ALIMENTO_CONSULTA_BD = "Info, Ejecutando consulta a base de datos para obtener alimentos";
        public const string LOG_ALIMENTO_OBTENER_LISTA_EXITOSO = "Success, Lista de alimentos obtenida correctamente";
        public const string LOG_ALIMENTO_OBTENER_LISTA_ERROR = "Error, Error al obtener la lista de alimentos";

        //ALIMENTO - VALIDACIONES
        public const string VAL_ALIMENTO_ID_INVALIDO = "Error, Validación fallida: El ID de alimento no es válido o no existe";
        public const string VAL_ALIMENTO_NOMBRE_VACIO = "Error, Validación fallida: El nombre del alimento no puede estar vacío";
        public const string VAL_ALIMENTO_NOMBRE_LONGITUD = "Error, Validación fallida: El nombre del alimento no cumple con la longitud requerida";
        public const string VAL_ALIMENTO_NOMBRE_DUPLICADO = "Error, Validación fallida: El alimento ya existe en la base de datos";
        public const string VAL_ALIMENTO_CALORIAS_INVALIDAS = "Error, Validación fallida: Las calorías no están dentro del rango permitido";
        public const string VAL_ALIMENTO_CALORIAS_NEGATIVAS = "Error, Validación fallida: Las calorías no pueden ser negativas";
        public const string VAL_ALIMENTO_PROTEINAS_INVALIDAS = "Error, Validación fallida: Las proteínas no están dentro del rango permitido";
        public const string VAL_ALIMENTO_PROTEINAS_NEGATIVAS = "Error, Validación fallida: Las proteínas no pueden ser negativas";
        public const string VAL_ALIMENTO_PROTEINAS_PRECISION = "Error, Validación fallida: Las proteínas exceden la precisión decimal permitida";
        public const string VAL_ALIMENTO_CARBOHIDRATOS_INVALIDOS = "Error, Validación fallida: Los carbohidratos no están dentro del rango permitido";
        public const string VAL_ALIMENTO_CARBOHIDRATOS_NEGATIVOS = "Error, Validación fallida: Los carbohidratos no pueden ser negativos";
        public const string VAL_ALIMENTO_CARBOHIDRATOS_PRECISION = "Error, Validación fallida: Los carbohidratos exceden la precisión decimal permitida";
        public const string VAL_ALIMENTO_GRASAS_INVALIDAS = "Error, Validación fallida: Las grasas no están dentro del rango permitido";
        public const string VAL_ALIMENTO_GRASAS_NEGATIVAS = "Error, Validación fallida: Las grasas no pueden ser negativas";
        public const string VAL_ALIMENTO_GRASAS_PRECISION = "Error, Validación fallida: Las grasas exceden la precisión decimal permitida";
        public const string VAL_ALIMENTO_MACRONUTRIENTES_INCONSISTENTES = "Warning, Validación fallida: Los macronutrientes no coinciden con las calorías declaradas";
        public const string VAL_ALIMENTO_OBJETO_NULO = "Error, Validación fallida: El objeto Alimento no puede ser nulo";
        public const string VAL_ALIMENTO_EN_USO = "Error, Validación fallida: El alimento no puede ser eliminado porque está siendo usado";
        public const string VAL_ALIMENTO_TODOS_VALORES_CERO = "Error, Validación fallida: El alimento no puede tener todos los valores nutricionales en cero";

        //MENU - VALIDACIONES
        public const string VAL_MENU_ID_INVALIDO = "Error, Validación fallida: El ID de menú no es válido o no existe";
        public const string VAL_MENU_FECHA_INVALIDA = "Error, Validación fallida: La fecha del menú no es válida";
        public const string VAL_MENU_FECHA_FUTURA_EXCESIVA = "Error, Validación fallida: No se pueden crear menús con tanta anticipación";
        public const string VAL_MENU_FECHA_PASADA_EXCESIVA = "Error, Validación fallida: No se pueden crear menús con fechas muy antiguas";
        public const string VAL_MENU_FECHA_DUPLICADA = "Error, Validación fallida: Ya existe un menú para la fecha especificada";
        public const string VAL_MENU_OBJETO_NULO = "Error, Validación fallida: El objeto Menu no puede ser nulo";
        public const string VAL_MENU_SIN_ALIMENTOS = "Error, Validación fallida: El menú debe tener al menos un alimento asociado";
        public const string VAL_MENU_EN_USO = "Error, Validación fallida: El menú no puede ser eliminado porque tiene alimentos asociados";

        //MENU ALIMENTO - VALIDACIONES
        public const string VAL_MENUALIMENTO_ID_INVALIDO = "Error, Validación fallida: El ID de MenuAlimento no es válido o no existe";
        public const string VAL_MENUALIMENTO_MENUID_INVALIDO = "Error, Validación fallida: El MenuId no es válido o no existe";
        public const string VAL_MENUALIMENTO_ALIMENTOID_INVALIDO = "Error, Validación fallida: El AlimentoId no es válido o no existe";
        public const string VAL_MENUALIMENTO_MENUID_VACIO = "Error, Validación fallida: El MenuId es requerido";
        public const string VAL_MENUALIMENTO_ALIMENTOID_VACIO = "Error, Validación fallida: El AlimentoId es requerido";
        public const string VAL_MENUALIMENTO_DUPLICADO = "Error, Validación fallida: El alimento ya está asociado al menú";
        public const string VAL_MENUALIMENTO_OBJETO_NULO = "Error, Validación fallida: El objeto MenuAlimento no puede ser nulo";
        public const string VAL_MENUALIMENTO_RELACION_INVALIDA = "Error, Validación fallida: La relación Menu-Alimento no puede ser establecida";

        // ============================================
        // MÓDULO: REGISTRO DE HÁBITOS
        // ============================================

        //REGISTRO ALIMENTO - LOGS DE OPERACIONES
        public const string LOG_REGISTROALIMENTO_REGISTRO_INICIO = "Info, Iniciando proceso de registro de consumo de alimento";
        public const string LOG_REGISTROALIMENTO_DTO_RECIBIDO = "Info, DTO recibido para registro de consumo";
        public const string LOG_REGISTROALIMENTO_VALIDACION_FALLIDA = "Warning, Validación fallida en registro de consumo";
        public const string LOG_REGISTROALIMENTO_SERVICIO_LLAMADA = "Info, Ejecutando servicio de registro de consumo";
        public const string LOG_REGISTROALIMENTO_REGISTRO_EXITOSO = "Success, Registro de consumo completado exitosamente";
        public const string LOG_REGISTROALIMENTO_REGISTRO_ERROR = "Error, Error interno al registrar consumo";

        public const string LOG_REGISTROALIMENTO_OBTENER_POR_ESTUDIANTE_INICIO = "Info, Solicitando registros de consumo para estudiante";
        public const string LOG_REGISTROALIMENTO_ESTUDIANTE_ID_RECIBIDO = "Info, Parámetro estudianteId recibido";
        public const string LOG_REGISTROALIMENTO_OBTENER_EXITOSO = "Success, Registros de consumo obtenidos correctamente";

        //TIPO COMIDA - LOGS DE OPERACIONES
        public const string LOG_TIPOCOMIDA_OBTENER_LISTA_INICIO = "Info, Iniciando proceso de obtención de tipos de comida";
        public const string LOG_TIPOCOMIDA_CONSULTA_BD = "Info, Ejecutando lectura de TiposComida";
        public const string LOG_TIPOCOMIDA_OBTENER_LISTA_EXITOSO = "Success, Tipos de comida obtenidos correctamente";

        //REGISTRO HABITO - LOGS DE OPERACIONES
        public const string LOG_REGISTROHABITO_REGISTRO_INICIO = "Info, Iniciando proceso de registro de hábitos";
        public const string LOG_REGISTROHABITO_DTO_RECIBIDO = "Info, DTO recibido para registro de hábitos";
        public const string LOG_REGISTROHABITO_VALIDACION_FALLIDA = "Warning, Validación fallida en registro de hábitos";
        public const string LOG_REGISTROHABITO_SERVICIO_LLAMADA = "Info, Llamando a servicio de registro de hábitos";
        public const string LOG_REGISTROHABITO_REGISTRO_EXITOSO = "Success, Hábitos registrados correctamente";
        public const string LOG_REGISTROHABITO_REGISTRO_ERROR = "Error, Error al registrar hábitos";

        public const string LOG_REGISTROHABITO_OBTENER_POR_ESTUDIANTE_INICIO = "Info, Solicitando hábitos para estudiante";
        public const string LOG_REGISTROHABITO_ESTUDIANTE_ID_RECIBIDO = "Info, Parámetro estudianteId recibido";
        public const string LOG_REGISTROHABITO_OBTENER_EXITOSO = "Success, Registros de hábitos encontrados";

        public const string LOG_REGISTROHABITO_OBTENER_TODOS_INICIO = "Info, Solicitando todos los hábitos por estudiante";
        public const string LOG_REGISTROHABITO_OBTENER_TODOS_EXITOSO = "Success, Registros de hábitos obtenidos exitosamente";

        public const string LOG_REGISTROHABITO_ACTUALIZAR_INICIO = "Info, Iniciando proceso de actualización de registro de hábito";
        public const string LOG_REGISTROHABITO_ACTUALIZAR_DTO_RECIBIDO = "Info, DTO recibido para actualización de hábito";
        public const string LOG_REGISTROHABITO_ACTUALIZAR_BODY_NULO = "Error, El cuerpo del request es nulo";
        public const string LOG_REGISTROHABITO_ACTUALIZAR_SERVICIO_LLAMADA = "Info, Ejecutando servicio de actualización de hábito";
        public const string LOG_REGISTROHABITO_ACTUALIZAR_EXITOSO = "Success, Registro de hábito actualizado exitosamente";
        public const string LOG_REGISTROHABITO_ACTUALIZAR_NO_ENCONTRADO = "Warning, No se encontró el registro de hábito a actualizar";

        //REGISTRO HABITO - VALIDACIONES
        public const string VAL_REGISTROHABITO_ID_INVALIDO = "Error, Validación fallida: El ID de RegistroHabito no es válido o no existe";
        public const string VAL_REGISTROHABITO_FECHA_INVALIDA = "Error, Validación fallida: La fecha del registro no es válida";
        public const string VAL_REGISTROHABITO_FECHA_FUTURA = "Error, Validación fallida: No se pueden registrar hábitos en el futuro";
        public const string VAL_REGISTROHABITO_FECHA_MUY_ANTIGUA = "Error, Validación fallida: No se pueden registrar hábitos con fechas muy antiguas";
        public const string VAL_REGISTROHABITO_ESTUDIANTEID_INVALIDO = "Error, Validación fallida: El EstudianteId no es válido o no existe";
        public const string VAL_REGISTROHABITO_ESTUDIANTEID_VACIO = "Error, Validación fallida: El EstudianteId es requerido";
        public const string VAL_REGISTROHABITO_DUPLICADO = "Error, Validación fallida: Ya existe un registro de hábito para el estudiante en la fecha especificada";
        public const string VAL_REGISTROHABITO_OBJETO_NULO = "Error, Validación fallida: El objeto RegistroHabito no puede ser nulo";
        public const string VAL_REGISTROHABITO_SIN_ALIMENTOS = "Error, Validación fallida: El registro de hábito debe tener al menos un alimento";
        public const string VAL_REGISTROHABITO_EN_USO = "Error, Validación fallida: El registro de hábito no puede ser eliminado porque tiene alimentos asociados";

        //TIPO COMIDA - VALIDACIONES
        public const string VAL_TIPOCOMIDA_ID_INVALIDO = "Error, Validación fallida: El ID de TipoComida no es válido o no existe";
        public const string VAL_TIPOCOMIDA_NOMBRE_VACIO = "Error, Validación fallida: El nombre del tipo de comida no puede estar vacío";
        public const string VAL_TIPOCOMIDA_NOMBRE_LONGITUD = "Error, Validación fallida: El nombre del tipo de comida no cumple con la longitud requerida";
        public const string VAL_TIPOCOMIDA_NOMBRE_DUPLICADO = "Error, Validación fallida: El tipo de comida ya existe en el sistema";
        public const string VAL_TIPOCOMIDA_OBJETO_NULO = "Error, Validación fallida: El objeto TipoComida no puede ser nulo";
        public const string VAL_TIPOCOMIDA_EN_USO = "Error, Validación fallida: El tipo de comida no puede ser eliminado porque está siendo usado";
        public const string VAL_TIPOCOMIDA_FORMATO = "Error, Validación fallida: El nombre del tipo de comida contiene caracteres no permitidos";

        //REGISTRO ALIMENTO - VALIDACIONES
        public const string VAL_REGISTROALIMENTO_ID_INVALIDO = "Error, Validación fallida: El ID de RegistroAlimento no es válido o no existe";
        public const string VAL_REGISTROALIMENTO_REGISTROHABITOID_INVALIDO = "Error, Validación fallida: El RegistroHabitoId no es válido o no existe";
        public const string VAL_REGISTROALIMENTO_ALIMENTOID_INVALIDO = "Error, Validación fallida: El AlimentoId no es válido o no existe";
        public const string VAL_REGISTROALIMENTO_TIPOCOMIDAID_INVALIDO = "Error, Validación fallida: El TipoComidaId no es válido o no existe";
        public const string VAL_REGISTROALIMENTO_REGISTROHABITOID_VACIO = "Error, Validación fallida: El RegistroHabitoId es requerido";
        public const string VAL_REGISTROALIMENTO_ALIMENTOID_VACIO = "Error, Validación fallida: El AlimentoId es requerido";
        public const string VAL_REGISTROALIMENTO_TIPOCOMIDAID_VACIO = "Error, Validación fallida: El TipoComidaId es requerido";
        public const string VAL_REGISTROALIMENTO_CANTIDAD_INVALIDA = "Error, Validación fallida: La cantidad no está dentro del rango permitido";
        public const string VAL_REGISTROALIMENTO_CANTIDAD_NEGATIVA = "Error, Validación fallida: La cantidad no puede ser negativa o cero";
        public const string VAL_REGISTROALIMENTO_OBJETO_NULO = "Error, Validación fallida: El objeto RegistroAlimento no puede ser nulo";
        public const string VAL_REGISTROALIMENTO_DUPLICADO = "Error, Validación fallida: El alimento ya está registrado para este hábito y tipo de comida";
        public const string VAL_REGISTROALIMENTO_CALORIAS_TOTALES_EXCESIVAS = "Warning, Validación fallida: El total de calorías excede el límite diario recomendado";

        // ============================================
        // MÓDULO: RECOMENDACIONES INTELIGENTES
        // ============================================

        //NUTRITION CALCULATE - LOGS DE OPERACIONES
        public const string LOG_NUTRITION_CALCULATE_INICIO = "Info, Iniciando proceso de cálculo nutricional";
        public const string LOG_NUTRITION_CALCULATE_DTO_RECIBIDO = "Info, NutritionRequestDTO recibido para cálculo";
        public const string LOG_NUTRITION_CALCULATE_EXITOSO = "Success, Cálculo nutricional completado exitosamente";
        public const string LOG_NUTRITION_CALCULATE_ARGUMENTO_INVALIDO = "Error, ArgumentException en cálculo nutricional";
        public const string LOG_NUTRITION_CALCULATE_ERROR = "Error, Error al ejecutar cálculo nutricional";

        //RECOMMENDATIONS - LOGS DE OPERACIONES
        public const string LOG_RECOMMENDATIONS_INICIO = "Info, Iniciando proceso de generación de recomendaciones inteligentes";
        public const string LOG_RECOMMENDATIONS_DTO_RECIBIDO = "Info, NutritionRequestDTO recibido para recomendaciones";
        public const string LOG_RECOMMENDATIONS_SERVICIO_NO_CONFIGURADO = "Error, El servicio de recomendaciones no está configurado";
        public const string LOG_RECOMMENDATIONS_EXITOSO = "Success, Recomendaciones generadas exitosamente";
        public const string LOG_RECOMMENDATIONS_ARGUMENTO_INVALIDO = "Error, ArgumentException al generar recomendaciones";
        public const string LOG_RECOMMENDATIONS_INVALID_OPERATION = "Error, InvalidOperationException al generar recomendaciones";
        public const string LOG_RECOMMENDATIONS_ERROR = "Error, Error interno al generar recomendaciones";

        // ============================================
        // MÓDULO: NOTIFICACIONES Y TESTING
        // ============================================

        public const string LOG_TESTEO_BD = "Success, Si ves esto es pq funciona bien";

        // ============================================
        // MÓDULO: LOGS Y AUDITORÍA
        // ============================================

        //LOGS - VALIDACIONES
        public const string VAL_LOG_ID_INVALIDO = "Error, Validación fallida: El ID de log no es válido o no existe";
        public const string VAL_LOG_TIPOACCIONID_INVALIDO = "Error, Validación fallida: El TipoAccionId no es válido o no existe";
        public const string VAL_LOG_RESULTADOID_INVALIDO = "Error, Validación fallida: El ResultadoId no es válido o no existe";
        public const string VAL_LOG_TIPOACCIONID_VACIO = "Error, Validación fallida: El TipoAccionId es requerido";
        public const string VAL_LOG_RESULTADOID_VACIO = "Error, Validación fallida: El ResultadoId es requerido";
        public const string VAL_LOG_FECHA_INVALIDA = "Error, Validación fallida: La fecha del log no es válida";
        public const string VAL_LOG_FECHA_FUTURA = "Error, Validación fallida: No se pueden registrar logs en el futuro";
        public const string VAL_LOG_USUARIOID_INVALIDO = "Error, Validación fallida: El UsuarioId no es válido o no existe";
        public const string VAL_LOG_ROL_LONGITUD = "Error, Validación fallida: El rol excede la longitud máxima permitida";
        public const string VAL_LOG_ENTIDAD_LONGITUD = "Error, Validación fallida: La entidad excede la longitud máxima permitida";
        public const string VAL_LOG_DETALLE_VACIO = "Error, Validación fallida: El detalle del log no puede estar vacío";
        public const string VAL_LOG_DETALLE_LONGITUD = "Error, Validación fallida: El detalle no cumple con la longitud requerida";
        public const string VAL_LOG_IP_FORMATO_INVALIDO = "Error, Validación fallida: El formato de la dirección IP no es válido";
        public const string VAL_LOG_IP_LONGITUD = "Error, Validación fallida: La IP excede la longitud máxima permitida";
        public const string VAL_LOG_OBJETO_NULO = "Error, Validación fallida: El objeto Log no puede ser nulo";

        //TIPO ACCION - VALIDACIONES
        public const string VAL_TIPOACCION_ID_INVALIDO = "Error, Validación fallida: El ID de TipoAccion no es válido o no existe";
        public const string VAL_TIPOACCION_NOMBRE_VACIO = "Error, Validación fallida: El nombre del tipo de acción no puede estar vacío";
        public const string VAL_TIPOACCION_NOMBRE_LONGITUD = "Error, Validación fallida: El nombre del tipo de acción no cumple con la longitud requerida";
        public const string VAL_TIPOACCION_NOMBRE_DUPLICADO = "Error, Validación fallida: El tipo de acción ya existe en el sistema";
        public const string VAL_TIPOACCION_OBJETO_NULO = "Error, Validación fallida: El objeto TipoAccion no puede ser nulo";
        public const string VAL_TIPOACCION_EN_USO = "Error, Validación fallida: El tipo de acción no puede ser eliminado porque está siendo usado";
        public const string VAL_TIPOACCION_FORMATO = "Error, Validación fallida: El nombre del tipo de acción contiene caracteres no permitidos";

        //TIPO RESULTADO - VALIDACIONES
        public const string VAL_TIPORESULTADO_ID_INVALIDO = "Error, Validación fallida: El ID de TipoResultado no es válido o no existe";
        public const string VAL_TIPORESULTADO_NOMBRE_VACIO = "Error, Validación fallida: El nombre del tipo de resultado no puede estar vacío";
        public const string VAL_TIPORESULTADO_NOMBRE_LONGITUD = "Error, Validación fallida: El nombre del tipo de resultado no cumple con la longitud requerida";
        public const string VAL_TIPORESULTADO_NOMBRE_DUPLICADO = "Error, Validación fallida: El tipo de resultado ya existe en el sistema";
        public const string VAL_TIPORESULTADO_OBJETO_NULO = "Error, Validación fallida: El objeto TipoResultado no puede ser nulo";
        public const string VAL_TIPORESULTADO_EN_USO = "Error, Validación fallida: El tipo de resultado no puede ser eliminado porque está siendo usado";
        public const string VAL_TIPORESULTADO_FORMATO = "Error, Validación fallida: El nombre del tipo de resultado contiene caracteres no permitidos";

        // ============================================
        // VALIDACIONES GENERALES
        // ============================================

        public const string VAL_CAMPO_REQUERIDO = "Error, Validación fallida: Campo requerido vacío";
        public const string VAL_FOREIGN_KEY_INVALIDA = "Error, Validación fallida: Clave foránea inválida";
        public const string VAL_RELACION_CASCADA_BLOQUEADA = "Error, Validación fallida: No se puede eliminar el registro porque tiene dependencias";
        public const string VAL_VALOR_FUERA_RANGO = "Error, Validación fallida: Valor fuera del rango permitido";
        public const string VAL_TIPO_DATO_INCORRECTO = "Error, Validación fallida: Tipo de dato incorrecto";
        public const string VAL_DECIMAL_PRECISION_EXCEDIDA = "Error, Validación fallida: Precisión decimal excedida";
        public const string VAL_LONGITUD_MAXIMA_EXCEDIDA = "Error, Validación fallida: Longitud máxima excedida";
        public const string VAL_INDICE_UNICO_VIOLADO = "Error, Validación fallida: Restricción de índice único violada";
        public const string VAL_FORMATO_FECHA_TIMEZONE = "Error, Validación fallida: Formato de fecha incorrecto";
        public const string VAL_REGISTRO_NO_ENCONTRADO = "Warning, Validación fallida: Registro no encontrado";
        public const string VAL_OPERACION_NO_PERMITIDA_RESTRICT = "Error, Validación fallida: Operación bloqueada por restricción de eliminación";
        public const string VAL_COLECCION_NULA = "Error, Validación fallida: Colección de navegación nula";
        public const string VAL_ENTIDAD_NO_RASTREADA = "Error, Validación fallida: Entidad no rastreada por el contexto";
        public const string VAL_CAMBIOS_CONCURRENTES = "Warning, Validación fallida: Conflicto de concurrencia detectado";

        // ============================================
        // INTEGRIDAD DE DATOS
        // ============================================

        public const string VAL_INTEGRIDAD_ESTUDIANTE_DATOS_COMPLETOS = "Error, Validación fallida: Datos incompletos del estudiante";
        public const string VAL_INTEGRIDAD_ALIMENTO_VALORES_COHERENTES = "Warning, Validación fallida: Valores nutricionales incoherentes";
        public const string VAL_INTEGRIDAD_MENU_FECHA_PASADA = "Error, Validación fallida: No se puede modificar un menú de fecha pasada";
        public const string VAL_INTEGRIDAD_LOG_AUDITORIA = "Error, Validación fallida: Los logs de auditoría no pueden ser modificados";
        public const string VAL_INTEGRIDAD_USUARIO_ROL_REQUERIDO = "Error, Validación fallida: El usuario debe tener un rol asignado";
        public const string VAL_INTEGRIDAD_REGISTROHABITO_FECHA_ESTUDIANTE = "Error, Validación fallida: Ya existe un registro de hábito para el estudiante en la fecha especificada";
    }
}