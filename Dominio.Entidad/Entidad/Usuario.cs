using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidad.Entidad
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string correo { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MinLength(6, ErrorMessage = "Debe tener al menos 6 caracteres")]
        public string clave { get; set; }

        [Display(Name = "Nombre completo")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nombre { get; set; }

        [Display(Name = "N° de Telefono")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Debe tener 9 dígitos")]
        public string telefono { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento")]
        public DateTime? fechaNacimiento { get; set; }

        [Display(Name = "Género")]
        public string genero { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool activo { get; set; }
    }
}
