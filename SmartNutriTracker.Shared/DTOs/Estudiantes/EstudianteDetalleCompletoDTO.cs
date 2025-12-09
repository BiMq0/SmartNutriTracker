namespace SmartNutriTracker.Shared.DTOs.Estudiantes;

public class EstudianteDetalleCompletoDTO
{
    public int EstudianteId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public int Edad { get; set; }
    public decimal IMC { get; set; }
    public List<RegistroHabitoDTO>? RegistroHabitos { get; set; }
}

public class RegistroHabitoDTO
{
    public int RegistroHabitoId { get; set; }
    public DateTime Fecha { get; set; }
    public decimal HorasSueno { get; set; }
    public decimal HorasActividadFisica { get; set; }
    public List<RegistroAlimentoDTO>? RegistroAlimentos { get; set; }
}

public class RegistroAlimentoDTO
{
    public int RegistroAlimentoId { get; set; }
    public AlimentoDTO? Alimento { get; set; }
    public int? Cantidad { get; set; }
    public TipoComidaDTO? TipoComida { get; set; }
}

public class AlimentoDTO
{
    public int AlimentoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Calorias { get; set; }
    public decimal Proteinas { get; set; }
    public decimal Carbohidratos { get; set; }
    public decimal Grasas { get; set; }
}

public class TipoComidaDTO
{
    public int TipoComidaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

