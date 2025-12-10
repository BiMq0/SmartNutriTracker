using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SmartNutriTracker.Shared.Validators
{
    /// <summary>
    /// Atributo de validación para contraseñas seguras
    /// Valida localmente en el cliente y servidor
    /// Estándares: OWASP, NIST SP 800-63B
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SecurePasswordAttribute : ValidationAttribute
    {
        private const int MinLength = 8;
        private const int MaxLength = 128;
        private const int MinLowercase = 1;
        private const int MinUppercase = 1;
        private const int MinDigits = 1;
        private const int MinSpecialChars = 1;

        // Patrones regex
        private static readonly Regex LowercaseRegex = new Regex(@"[a-z]");
        private static readonly Regex UppercaseRegex = new Regex(@"[A-Z]");
        private static readonly Regex DigitRegex = new Regex(@"\d");
        private static readonly Regex SpecialCharRegex = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':"",.<>?/\\|`~]");
        private static readonly Regex SequenceRegex = new Regex(@"(?:012|123|234|345|456|567|678|789|890|abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz)", RegexOptions.IgnoreCase);

        private static readonly HashSet<string> CommonPasswords = new(StringComparer.OrdinalIgnoreCase)
        {
            "password", "123456", "12345678", "qwerty", "abc123", "monkey", "1234567",
            "letmein", "trustno1", "dragon", "baseball", "111111", "iloveyou", "master",
            "sunshine", "ashley", "bailey", "passw0rd", "shadow", "123123", "654321",
            "superman", "qazwsx", "michael", "football", "usuario", "admin", "root",
            "smartnutri", "nutricion", "nutritrac", "smartnutitracker"
        };

        public SecurePasswordAttribute()
        {
            ErrorMessage = "La contraseña no cumple con los requisitos de seguridad.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("La contraseña no puede estar vacía.");
            }

            string password = value.ToString()!;
            var errors = new List<string>();

            // Validar longitud
            if (password.Length < MinLength)
            {
                errors.Add($"Mínimo {MinLength} caracteres.");
            }

            if (password.Length > MaxLength)
            {
                errors.Add($"Máximo {MaxLength} caracteres.");
            }

            // Validar minúsculas
            if (LowercaseRegex.Matches(password).Count < MinLowercase)
            {
                errors.Add("Al menos 1 letra minúscula (a-z).");
            }

            // Validar mayúsculas
            if (UppercaseRegex.Matches(password).Count < MinUppercase)
            {
                errors.Add("Al menos 1 letra mayúscula (A-Z).");
            }

            // Validar dígitos
            if (DigitRegex.Matches(password).Count < MinDigits)
            {
                errors.Add("Al menos 1 número (0-9).");
            }

            // Validar caracteres especiales
            if (SpecialCharRegex.Matches(password).Count < MinSpecialChars)
            {
                errors.Add("Al menos 1 carácter especial (!@#$%^&*...).");
            }

            // Verificar secuencias comunes
            if (SequenceRegex.IsMatch(password))
            {
                errors.Add("Evita secuencias comunes (abc, 123, qwerty).");
            }

            // Verificar repetición excesiva
            if (HasExcessiveRepeatingCharacters(password))
            {
                errors.Add("No repitas caracteres más de 3 veces (aaaa, 1111).");
            }

            // Verificar contraseñas comunes
            if (CommonPasswords.Contains(password))
            {
                errors.Add("Esta contraseña es muy común. Elige una más única.");
            }

            if (errors.Count > 0)
            {
                return new ValidationResult($"Requisitos de seguridad:\n• {string.Join("\n• ", errors)}");
            }

            return ValidationResult.Success;
        }

        private static bool HasExcessiveRepeatingCharacters(string password)
        {
            for (int i = 0; i < password.Length - 3; i++)
            {
                if (password[i] == password[i + 1] &&
                    password[i] == password[i + 2] &&
                    password[i] == password[i + 3])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
